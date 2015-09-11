var adminPageController = function ($scope) {
    $scope.selectedMainMenu = '/Clients';
    $scope.selectedCLientsMenu = '/AddUser';
    $scope.helloVariable = 'I work!';
    $scope.mainMenu = []
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
}