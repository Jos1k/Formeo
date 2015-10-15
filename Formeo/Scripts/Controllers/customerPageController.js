var customerPageController = function ($scope, $window, $http, $modal, UploadPrinObject) {

    $scope.selectedMainMenu = '/Dashboard';
    $scope.selectedClientsMenu = '/AddUser';
    $scope.helloVariable = 'I work!';
    $scope.mainMenu = [];
    $scope.menuType = "0";
    $scope.selectedPrintObjectIds = [];
    $scope.layOrderButtonIsDisabled = true;

    $scope.addOrRemovePrintobject = function(printObjectId) {
        var index = $scope.selectedPrintObjectIds.indexOf(printObjectId);

        if (index > -1) {
            $scope.selectedPrintObjectIds.splice(index, 1);
        } else {
            $scope.selectedPrintObjectIds.push(printObjectId);
        }
        $scope.layOrderButtonIsDisabled = $scope.selectedPrintObjectIds.length > 0 ? false : true;
    };

    $scope.printObjectIsSelected = function (printObjectId) {
        return $scope.selectedPrintObjectIds.indexOf(printObjectId) > -1;
    };

    $scope.showLayOrderModal = function() {

        if ($scope.isActiveMainMenu('/Storage')) {
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
                then(function(response) {
                    //success
                    var modalInstance = $modal.open({
                        template: (response.data),
                        controller: 'layOrderPartialController',
                        backdrop: 'static',
                        windowClass: 'app-modal-window-lay-order'
                    });

                    modalInstance.result.then(function(response) {
                        $scope.activeProjects.push(response);
                        $scope.selectedPrintObjectIds = [];
                        $scope.layOrderButtonIsDisabled = true;
                    }, function(response) {
                        //error
                        $modalInstance.dismiss('cancel');

                    });

                    $scope.layOrderButtonIsDisabled = false;


                }, function(response) {
                    //error
                    $window.alert('error');
                    $scope.layOrderButtonIsDisabled = false;

                });
        } else {
            $scope.selectMainMenu('/Storage');
        }
    };

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

    $scope.toogleIsNeedBid = function (printObject) {


        $http({
            method: 'POST',
            url: '/PrintObjects/ToggleIsNeedPrintObjectBid',
            params: { 'printObjectId': printObject.Id },
            headers: { 'Content-Type': 'application/x-www-form-urlencoded' }
        }).then(function (responce) {

            printObject.IsNeedBid = responce.data
        }, function (response) {
            //error
            $window.alert('error');


        });

    }


    $scope.ShowBidsForPrintObject = function (printObjectId) {

        $http({
            method: 'GET',
            url: '/Bids/GetBidsForPrintObject',
            params: { 'printObjectId': printObjectId },
            headers: { 'Content-Type': 'application/x-www-form-urlencoded' }
        }).
                then(function (response) {
                    //success
                    var modalInstance = $modal.open({
                        template: (response.data),
                        controller: 'bidsForPrintObjectController',
                        backdrop: 'static',
                        resolve: {
                            printObjectId: function () { return printObjectId; }
                        }
                    });

                    modalInstance.result.then(function (response) {

                        for (var i = 0; i < $scope.printObjects.length; i++) {
                            if ($scope.printObjects[i].Id == printObjectId) {
                                $scope.printObjects[i] =JSON.parse(  response);
                            }
                        }

                        $scope.printObjects.push(response);
                        $scope.selectedPrintObjectIds = [];
                    }, function (response) {
                        //error
                        $modalInstance.dismiss('cancel');

                    });



                }, function (response) {
                    //error
                    $window.alert('error');

                });
    }

    $scope.showUploadProductModal = function () {
        UploadPrinObject.showUpload($scope.printObjects);
    }

}