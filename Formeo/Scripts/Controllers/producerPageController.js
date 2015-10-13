var producerPageController = function ($scope, $http, $modal, UploadPrinObject) {
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
    $scope.selectedPrintObjectToBidId = "";

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

    $scope.printObjectToBidIsSelected = function (printObjectId) {
        return $scope.selectedPrintObjectToBid.id == printObjectId;
    };

    $scope.selectSingleItem = function (position, entities) {
        angular.forEach(entities, function (printObject, index) {
            if (position != index) {
                printObject.IsSelected = false;
            }
            else {
                $scope.selectedPrintObjectToBidId = printObject.Id;
            }
        });
    };

    $scope.showBidProductModal = function (printObjectId) {

        if (printObjectId == "" || printObjectId <= 0) { return; }

        $http({
            method: 'GET',
            url: '/Bids/GetBidModal',
            params: { 'printObjectId': printObjectId },
            headers: { 'Content-Type': 'application/x-www-form-urlencoded' }
        }).
                then(function (response) {
                    //success
                    var modalInstance = $modal.open({
                        template: (response.data),
                        controller: 'bidProductPartialController',
                        backdrop: 'static'
                    });

                    modalInstance.result.then(function (response) {
                        //modal success

                        $scope.removeItemById($scope.needBidPrintObjects, printObjectId);
                    }, function (response) {
                        //modal error
                        $modalInstance.dismiss('cancel');

                    });



                }, function (response) {
                    //http error
                    $window.alert('error');
                });
    }

    $scope.removeItemById = function (elements, id) {
        angular.forEach(elements, function (element, index) {
            if (element.Id == id) {
                elements.splice(index, 1);
            }
        });
    }

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

    $scope.showUploadProductModal = function () {
        var newPrintObjects = UploadPrinObject.showUpload();
        $scope.printObjects.push(newPrintObjects);
    }
}