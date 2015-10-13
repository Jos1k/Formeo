using System.Collections;
using System.Collections.Generic;
using D2L.Data;
using D2L.LE.Quiz.Domain;
using D2L.LE.Quizzes;
using D2L.LE.Quizzes.Domain;
using D2L.Lms.QuestionCollection;
using D2L.Lms.Quizzing;
using D2L.LP.Configuration.FeatureToggling.Domain;
using D2L.LP.Extensibility.Activation.Domain;
using D2L.LP.OrgUnits;
using D2L.LP.OrgUnits.Domain;
using D2L.LP.Security.Authorization.Roles;
using D2L.LP.TestFramework.Configuration.FeatureToggling.Domain;
using D2L.LP.Users.Management;
using D2L.LE.Quizzing.Tests.Utilities;
using D2L.LE.Quizzing.Tests.Utilities.Questions;
using NUnit.Framework;

namespace D2L.LE.Quizzing.Tests.Integration.Database_QuizzingProvider {
	[TestFixture]
	[Category( "Integration" )]
	public class Database_QuizzingProviderTests {
		private ITestCourse m_course;
		private ITestUser m_studentCommonAccessUser;
		private ITestUser m_studentSpecialAccessUser;
		private ITestRole m_studentRole;
		//Need to be refactored. Get rid of global contexts
		private IGlobalContext m_studentCommonContext;
		private IGlobalContext m_studentSpecialGlobalContext;
		private IQuizzesManager m_quizManager;
		private ITestFeatureToggler m_attemptsSpecialAccessFeature;
		private IQuiz m_quiz;
		private IQuizRestrictionsSpecialAccessEntity m_quizRestrictionsSpecialAccessEntity;

		private IReadOnlyList<StudentAnswer> m_quizAnswers = new[]{
			new StudentAnswer{
				FirtQuestionAnswer = true,
				SecondQuestionAnswer = true
			},
			new StudentAnswer{
				FirtQuestionAnswer = false,
				SecondQuestionAnswer = true
			},
			new StudentAnswer{
				FirtQuestionAnswer = false,
				SecondQuestionAnswer = false
			},
			new StudentAnswer{
				FirtQuestionAnswer = true,
				SecondQuestionAnswer = false
			}
		};

		#region TestCases
		private static IEnumerable QuizzOveralCalculationTestCases {
			get {
				yield return new TestCaseData( 2, 1, false, 0, false, 2m )
					.SetName( "CommonAccess_FeatureToggleOff_TwoAttempts_HighestAttempt_2Expected" );
				yield return new TestCaseData( 3, 2, false, 0, false, 0m )
					.SetName( "CommonAccess_FeatureToggleOff_ThreeAttempts_LowestAttempt_0Expected" );
				yield return new TestCaseData( 3, 3, false, 0, false, 1m )
					.SetName( "CommonAccess_FeatureToggleOff_ThreeAttempts_AverageAttempts_1Expected" );
				yield return new TestCaseData( 3, 4, false, 0, false, 2m )
					.SetName( "CommonAccess_FeatureToggleOff_ThreeAttempts_FirstAttempt_2Expected" );
				yield return new TestCaseData( 3, 3, true, 2, true, 0m )
					.SetName( "SpecialAccess_FeatureToggleOn_ThreeAttempts_LowestAttemptSpecial_AverageAttemptCommon_0Expected" );
				yield return new TestCaseData( 3, 3, true, 1, true, 2m )
					.SetName( "SpecialAccess_FeatureToggleOn_ThreeAttempts_HighestAttemptsSpecial_AverageAttemptCommon_2Expected" );
				yield return new TestCaseData( 2, 3, true, 4, true, 2m )
					.SetName( "SpecialAccess_FeatureToggleOn_TwoAttempts_FirstAttemptSpecial_AverageAttemptCommon_2Expected" );
				yield return new TestCaseData( 3, 1, true, 3, true, 1m )
					.SetName( "SpecialAccess_FeatureToggleOn_ThreeAttempts_AverageAttemptSpecial_HighestAttemptCommon_1Expected" );
				yield return new TestCaseData( 3, 1, true, 3, false, 2m )
					.SetName( "SpecialAccess_FeatureToggleOff_ThreeAttempts_AverageAttemptSpecial_HighestAttemptCommon_2Expected" );
			}
		}

		private static IEnumerable QuizzOveralCalculationTestCasesWithChangingCaclTypeId {
			get {
				yield return new TestCaseData( 2, 1, 2, false, 0, false, 2m, 1m )
					.SetName( "CommonAccess_FeatureToggleOff_TwoAttempts_HighestAttemptBefore_100PercentsExpected_LowestAttemptAfter_50PercentsExpected" );
				yield return new TestCaseData( 2, 1, 2, true, 3, true, 1.5m, 1m )
					.SetName( "SpecialAccess_FeatureToggleOn_TwoAttempts_AverageBefore_75PercentsExpected_LowestAttemptAfter_50PercentsExpected" );
				yield return new TestCaseData( 2, 1, 2, true, 3, false, 2m, 1m )
					.SetName( "SpecialAccess_FeatureToggleOff_TwoAttempts_HighestAttemptBefore_100PercentsExpected_LowestAttemptAfter_50PercentsExpected" );
			}
		}

		private static IEnumerable QuizzAllowedAttemptsTestCases {
			get {
				yield return new TestCaseData( 0, 3, false, 2, false, 3 )
					.SetName( "CommonAccess_FeatureToggleOff_ZeroPassedAttempts_ExpectedThreeAvailableAttempts" );
				yield return new TestCaseData( 2, 3, false, 2, false, 1 )
					.SetName( "CommonAccess_FeatureToggleOff_TwoPassedAttepmts_ExpectedOneAvailableAttempt" );
				yield return new TestCaseData( 4, 3, false, 2, false, 0 )
					.Throws( typeof( DbProviderException ) )
					.SetName( "CommonAccess_FeatureToggleOff_FourPassedAttempts_ExpectedPassingQuizMoreTimesThanAllowedException" );
				yield return new TestCaseData( 0, 3, true, 4, true, 4 )
					.SetName( "SpecialAccess_FeatureToggleOn_ZeroPassedAttempts_ExpectedFourAvailableAttempts" );
				yield return new TestCaseData( 1, 3, true, 4, true, 3 )
					.SetName( "SpecialAccess_FeatureToggleOn_OnePassedAttepmts_ExpectedThreeAvailableAttempt" );
				yield return new TestCaseData( 5, 3, true, 4, true, 0 )
					.Throws( typeof( DbProviderException ) )
					.SetName( "SpecialAccess_FeatureToggleOn_FivePassedAttempts_ExpectedPassingQuizMoreTimesThanAllowedException" );
				yield return new TestCaseData( 3, 3, true, 4, false, 0 )
					.SetName( "SpecialAcess_FeatureToggleOff_TreePassedAttempts_ExpectedZeroAvailableAttempt" );
			}
		}

		#endregion

		[TestFixtureSetUp]
		public void TestFixtureSetUp() {
			m_attemptsSpecialAccessFeature = TestFeatureTogglerFactory.Create();
			m_course = TestOrgUnitFactory.CreateCourse( TestOrg.DevId );
			m_studentRole = TestRoleFactory.CreateRole( TestOrg.DevId );
			m_studentCommonAccessUser = TestUserFactory.CreateUser( TestOrg.DevId );
			m_studentSpecialAccessUser = TestUserFactory.CreateUser( TestOrg.DevId );

			m_quizManager = TestServiceLocatorFactory
				.Create(
					m_course,
					TestUserFactory.D2LSupport
				)
				.Get<IQuizzesManager>();

			m_course.EnrollUser( m_studentCommonAccessUser, m_studentRole );
			m_course.EnrollUser( m_studentSpecialAccessUser, m_studentRole );
			RoleUtility.SetPermissions(
				m_studentRole.RoleId,
				SeeQuizzing: true,
				TakeQuizzes: true
			);

			m_studentCommonContext = TestServiceLocatorFactory
				.Create(
					m_course,
					m_studentCommonAccessUser
				)
				.Get<IGlobalContext>();

			m_studentSpecialGlobalContext = TestServiceLocatorFactory
				.Create(
					m_course,
					m_studentSpecialAccessUser
				)
				.Get<IGlobalContext>();
		}

		[TestFixtureTearDown]
		public void TestFixtureTearDown() {
			m_course.SafeDispose();
			m_studentRole.SafeDispose();
			m_studentCommonAccessUser.SafeDispose();
			m_attemptsSpecialAccessFeature.Reset<IAttemptsSpecialAccessFeature>();
		}

		[TearDown]
		public void TearDown() {
			QuizzingTool.CreateProvider( m_studentSpecialGlobalContext )
				.DeleteSpecialAccess( m_quiz.QuizId, m_studentSpecialGlobalContext.User.UserId );
		}

		[Test, TestCaseSource( "QuizzAllowedAttemptsTestCases" )]
		public void CheckQuizzAllowedAttemptsCount(
			int takeQuizCount,
			int quizAllowedAttempts,
			bool isSpecialAccess,
			int specialAllowedAttempts,
			bool enableFeatureToggle,
			int expectedAttemptsLeft
			) {
			IGlobalContext userGlobalContext = m_studentCommonContext;
			QuizzingTestState state = CreateQuizWithQuestions(
				allowedAttempts: quizAllowedAttempts,
				overalCalculationTypeId: 2
			);

			SetFeatureState( enableFeatureToggle );
			if( isSpecialAccess ) {
				SetSpecialAccessToUser( state.QuizId, calcTypeId: 1, attemptsAllowed: specialAllowedAttempts );
				userGlobalContext = m_studentSpecialGlobalContext;
			}
			for( int i = 0; i < takeQuizCount; i++ ) {
				TakeQuiz( state, userGlobalContext, m_quizAnswers[i] );
			}

			int attemptsAllowed = QuizUtility.GetUserAvailableAttemptsCount( userGlobalContext, state.QuizId );
			Assert.AreEqual( expectedAttemptsLeft, attemptsAllowed );
		}

		[Test, TestCaseSource( "QuizzOveralCalculationTestCases" )]
		public void CheckQuizzOveralCalculationScore(
				int takeQuizCount,
				int quizCalcTypeId,
				bool hasSpecialAccess,
				int specialQuizCalcTypeId,
				bool enableFeatureToggleState,
				decimal expectedCalculationResult
			) {
			IQuizUserScore quizUserScore;
			IGlobalContext studentGlobalContext = m_studentCommonContext;
			QuizzingTestState state = CreateQuizWithQuestions(
				allowedAttempts: 3,
				overalCalculationTypeId: quizCalcTypeId
			);

			SetFeatureState( enableFeatureToggleState );
			if( hasSpecialAccess ) {
				SetSpecialAccessToUser( state.QuizId, specialQuizCalcTypeId, attemptsAllowed: 3 );
				studentGlobalContext = m_studentSpecialGlobalContext;
			}
			for( int i = 0; i < takeQuizCount; i++ ) {
				TakeQuiz( state, studentGlobalContext, m_quizAnswers[i] );
			}
			m_quizManager.TryFetchUserScore( state.QuizId, studentGlobalContext.User.UserId, out quizUserScore );

			Assert.AreEqual( expectedCalculationResult, quizUserScore.Score );
		}

		[Test, TestCaseSource( "QuizzOveralCalculationTestCasesWithChangingCaclTypeId" )]
		public void CheckQuizzOveralCalculationPercentScoreAfterChangedCalcTypeId(
				int takeQuizCount,
				int quizCalcTypeId,
				int quizNewCalcTypeId,
				bool hasSpecialAccess,
				int specialQuizCalcTypeId,
				bool enableFeatureToggle,
				decimal expectedCalculationResult,
				decimal expecedCalculationResultAfterChangedCalcTypeId
			) {
			IQuizUserScore quizUserScore;
			IGlobalContext studentGlobalContext = m_studentCommonContext;
			QuizzingTestState state = CreateQuizWithQuestions(
				allowedAttempts: 3,
				overalCalculationTypeId: quizCalcTypeId
			);

			SetFeatureState( enableFeatureToggle );
			if( hasSpecialAccess ) {
				SetSpecialAccessToUser( state.QuizId, specialQuizCalcTypeId, attemptsAllowed: 4 );
				studentGlobalContext = m_studentSpecialGlobalContext;
			}
			for( int i = 0; i < takeQuizCount; i++ ) {
				TakeQuiz( state, studentGlobalContext, m_quizAnswers[i] );
			}

			m_quizManager.TryFetchUserScore( state.QuizId, studentGlobalContext.User.UserId, out quizUserScore );

			Assert.AreEqual( expectedCalculationResult, quizUserScore.Score );

			UpdateQuizzOverallCalcType( studentGlobalContext, state.QuizId, quizNewCalcTypeId, hasSpecialAccess, enableFeatureToggle );
			m_quizManager.TryFetchUserScore( state.QuizId, studentGlobalContext.User.UserId, out quizUserScore );

			Assert.AreEqual( expecedCalculationResultAfterChangedCalcTypeId, quizUserScore.Score );
		}

		private void SetFeatureState( bool enableFeatureToggleState ) {
			if( enableFeatureToggleState ) {
				m_attemptsSpecialAccessFeature.Enable<IAttemptsSpecialAccessFeature>();
			} else {
				m_attemptsSpecialAccessFeature.Disable<IAttemptsSpecialAccessFeature>();
			}
		}

		private QuizzingTestState CreateQuizWithQuestions( int allowedAttempts, int overalCalculationTypeId ) {
			QuizzingTestState state = new QuizzingTestState();
			m_quiz = TestQuizFactory.Create( m_course );
			m_quiz.AttemptsAllowed = allowedAttempts;
			m_quiz.CalcTypeId = overalCalculationTypeId;
			m_quiz.Save();

			state.QuizId = m_quiz.QuizId;

			state.Question1 = TrueFalseQuestionUtility.Create(
				m_studentCommonContext,
				state.QuizId,
				answerIsTrue: true
			);
			state.Question2 = TrueFalseQuestionUtility.Create(
				m_studentCommonContext,
				state.QuizId,
				answerIsTrue: true
			);
			return state;
		}

		private static void TakeQuiz( QuizzingTestState state, IGlobalContext studentGlobalContext, StudentAnswer studentAnswer ) {
			long attemptId = AttemptUtility.Create( studentGlobalContext, state.QuizId );

			TrueFalseQuestionUtility.Answer(
					studentGlobalContext,
					state.QuizId,
					attemptId,
					state.Question1,
					answer: studentAnswer.FirtQuestionAnswer
				);

			TrueFalseQuestionUtility.Answer(
					studentGlobalContext,
					state.QuizId,
					attemptId,
					state.Question2,
					answer: studentAnswer.SecondQuestionAnswer
				);

			AttemptUtility.Submit( studentGlobalContext, state.QuizId, attemptId );
		}

		private void UpdateQuizzOverallCalcType( IGlobalContext global, long quizId, int calcTypeId, bool hasSpecialAccess, bool enableFeatureToggle ) {
			if( !hasSpecialAccess || !enableFeatureToggle ) {
				IQuizzingProvider quizzingProvider = QuizzingTool.CreateProvider( global );

				QuizData quizdata = new QuizData();
				quizdata.CalcTypeId.Val = calcTypeId;
				quizdata.OrgUnitId.Val = global.OrgUnit.OrgUnitId;
				quizzingProvider.UpdateQuiz( quizId, global.User.UserId, quizdata );
			} else {
				m_quizRestrictionsSpecialAccessEntity.CalcTypeId = calcTypeId;
				m_quizRestrictionsSpecialAccessEntity.Save();
			}
		}

		private void SetSpecialAccessToUser( long quizId, int calcTypeId, int attemptsAllowed ) {
			m_quizRestrictionsSpecialAccessEntity = m_quizManager
				.CreateQuizRestrictionsSpecialAccess();

			SpecialAccessData m_specialAccessData = new SpecialAccessData {
				StartDate = new D2LDateTimeField {
					Val = new D2LDateTime( GlobalContext.Anonymous, UtcDateTime.Now.AddDays( -10 ) )
				},
				EndDate = new D2LDateTimeField {
					Val = new D2LDateTime( GlobalContext.Anonymous, UtcDateTime.Now.AddDays( 10 ) )
				}
			};

			QuizzingTool.CreateProvider( m_studentSpecialGlobalContext )
				.CreateSpecialAccess(
				quizId,
				m_specialAccessData,
				m_studentSpecialAccessUser.UserId
				);

			m_quizRestrictionsSpecialAccessEntity.QuizId = quizId;
			m_quizRestrictionsSpecialAccessEntity.AttemptsAllowed = attemptsAllowed;
			m_quizRestrictionsSpecialAccessEntity.CalcTypeId = calcTypeId;
			m_quizRestrictionsSpecialAccessEntity.UserId = m_studentSpecialAccessUser.UserId;
			m_quizRestrictionsSpecialAccessEntity.Save();
		}

		private sealed class StudentAnswer {
			public bool FirtQuestionAnswer { get; set; }
			public bool SecondQuestionAnswer { get; set; }
		}

		private class QuizzingTestState {
			public long QuizId { get; set; }
			public QuestionId Question1 { get; set; }
			public QuestionId Question2 { get; set; }
		}
	}
}
