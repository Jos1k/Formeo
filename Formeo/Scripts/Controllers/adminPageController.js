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
    $scope.selectedClientsMenu = '/AddUser';
    $scope.helloVariable = 'I work!';
    $scope.mainMenu = [];
    $scope.menuType = "0";
    $scope.selectedUser = $scope.EMPTY;

    $scope.updateUserInfo = function (isShownEditableTextbox, user, fieldNameToChange, newValue) {

        if(!isShownEditableTextbox ||  user == $scope.EMPTY)
        {
            $window.alert('error');
            return;
        }

        $http({
            method: 'POST',
            url: '/Account/UpdateUserInfo',
            params: { userName: user.UserName, field: fieldNameToChange, newValue: newValue },
            headers: { 'Content-Type': 'application/x-www-form-urlencoded' }
        }).
                then(function (response) {
                    //success
                }, function (response) {
                    //error
                    $window.alert('error');
                });
    }

    $scope.removeUser = function (user, collectionToHandle) {


        if (user != $scope.EMPTY) {

            $http({
                method: 'POST',
                url: '/Account/RemoveUser',
                params: { userName: user.UserName },
                headers: { 'Content-Type': 'application/x-www-form-urlencoded' }
            }).
              then(function (response) {
                  var index = collectionToHandle.indexOf(user);
                  collectionToHandle.splice(index, 1);
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
        $scope.selectedClientsMenu = item;
    };

    $scope.isActiveCLientsMenu = function (item) {
        return $scope.selectedClientsMenu === item;
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