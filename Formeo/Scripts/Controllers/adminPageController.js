var adminPageController = function ($scope) {
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

    $scope.selectUser = function (user)
    {
        if (user != $scope.EMPTY)
        {
            $scope.selectedUser = user;
        }
    }

    $scope.isMenuTypeIs = function (type)
    {
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