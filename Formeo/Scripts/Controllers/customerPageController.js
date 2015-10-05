﻿var customerPageController = function ($scope, $window, $http, $modal) {
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

    $scope.selectedMainMenu = '/Dashboard';
    $scope.selectedClientsMenu = '/AddUser';
    $scope.helloVariable = 'I work!';
    $scope.mainMenu = [];
    $scope.menuType = "0";
    $scope.selectedPrintObjectIds = [];
    $scope.layOrderButtonIsDisabled = true;

    $scope.addOrRemovePrintobject = function (printObjectId) {
        var index = $scope.selectedPrintObjectIds.indexOf(printObjectId);

        if (index > -1) {
            $scope.selectedPrintObjectIds.splice(index, 1);
        }
        else {
            $scope.selectedPrintObjectIds.push(printObjectId);
        }
        $scope.layOrderButtonIsDisabled = $scope.selectedPrintObjectIds.length > 0 ? false : true;
    }

    $scope.printObjectIsSelected = function (printObjectId) {
        return $scope.selectedPrintObjectIds.indexOf(printObjectId) > -1;
    }

    $scope.showLayOrderModal = function () {

        if (!$scope.selectedPrintObjectIds.length) {
            return;
        }

        $scope.layOrderButtonIsDisabled = true;

        $http({
            method: 'GET',
            url: '/Project/LayOrder',
            params: { 'selectedPrintObjectIds': $scope.selectedPrintObjectIds },
            headers: { 'Content-Type': 'application/x-www-form-urlencoded' }
        }).
                then(function (response) {
                    //success
                    var modalInstance = $modal.open({
                        template: (response.data),
                        controller: 'layOrderPartialController',
                        backdrop: 'static'
                    });

                    modalInstance.result.then(function (response) {
                        $scope.activeProjects.push(response);
                    }, function (response) {
                        //error
                        $modalInstance.dismiss('cancel');

                    });

                    $scope.layOrderButtonIsDisabled = false;


                }, function (response) {
                    //error
                    $window.alert('error');
                    $scope.layOrderButtonIsDisabled = false;

                });
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