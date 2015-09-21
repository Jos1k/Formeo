var adminPageController = function ($scope, $window, $http) {
    $scope.userModel = {
        username: '',
        password: '',
        email: '',
        address: '',
        postal: '',
        country: '',
        isProduction: false,
        isCustomer: false,
        isAdmin: false
    };

    $scope.EMPTY = "";

    $scope.selectedMainMenu = '/Clients';
    $scope.selectedCLientsMenu = '/AddUser';
    $scope.helloVariable = 'I work!';
    $scope.mainMenu = [];
    $scope.menuType = "0";
    $scope.selectedUser = $scope.EMPTY;

    $scope.removeUser = function (user, collectionToHandle) {

        $window.alert(collectionToHandle.length);

        if (user != $scope.EMPTY) {
            
            $http({
                method: 'POST',
                url: '/Account/RemoveUser',
                params: { userName: user.UserName },
                headers: { 'Content-Type': 'application/x-www-form-urlencoded' }
            }).
              then(function (response) {
                  // this callback will be called asynchronously
                  // when the response is available
              }, function (response) {
                  // called asynchronously if an error occurs
                  // or server returns response with an error status.
              });
        }
    }

    $scope.selectUser = function (user) {
        if (user != $scope.EMPTY) {
            $scope.selectedUser = user;
        }
    }

    $scope.isMenuTypeIs = function (type) {
        return $scope.menuType === type;
    };

    $scope.selectMainMenu = function (item) {
        $scope.selectedMainMenu = item;
    };

    $scope.isActiveMainMenu = function (item) {
        return $scope.selectedMainMenu === item;
    };

    $scope.selectClientMenu = function (item) {
        $scope.selectedCLientsMenu = item;
    };

    $scope.isActiveCLientsMenu = function (item) {
        return $scope.selectedCLientsMenu === item;
    };

    $scope.cleanUserModel = function () {
        $scope.userModel = {
            username: '',
            password: '',
            email: '',
            address: '',
            postal: '',
            country: '',
            isProduction: false,
            isCustomer: false,
            isAdmin: false
        };
    };
}