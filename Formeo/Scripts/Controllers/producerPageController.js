var producerPageController = function ($scope, $http, $modal, UploadPrinObject) {
    UploadPrinObject.storeScope($scope, 'printObjects');
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
        if ($scope.isActiveMainMenu('/Dashboard')) {

            if (printObjectId == "" || printObjectId <= 0) {
                return;
            }

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
        else {
            $scope.selectMainMenu('/Dashboard');
        }
    };

    $scope.removeItemById = function (elements, id) {
        angular.forEach(elements, function (element, index) {
            if (element.Id == id) {
                elements.splice(index, 1);
            }
        });
    }

    $scope.SetProductionStatus = function (projectInfo, status) {
        $http({
            method: 'POST',
            url: '/Project/SetProductState',
            params: { 'projectId': projectInfo.ProjectId, 'printObjectId': projectInfo.PrinObjectID, 'status': status },
            headers: { 'Content-Type': 'application/x-www-form-urlencoded' }
        }).
               then(function (response) {
                   //success

                   $scope.UpdateProductOnUI(projectInfo, status);

               }, function (response) {
                   //http error

               });
    }

    $scope.ShowProductDetails = function (printObject) {
        $http({
            method: 'GET',
            url: '/Project/GetProjectInfoDetails',
            params: { 'projectId': printObject.ProjectId, 'printObjectId': printObject.PrinObjectID },
            headers: { 'Content-Type': 'application/x-www-form-urlencoded' }
        }).
               then(function (response) {
                   //success
                   var modalInstance = $modal.open({
                       template: (response.data),
                       controller: 'printObjectDetailsPartialController',
                       backdrop: 'static'
                   });
               }, function (response) {
                   //http error

               });
    }

    $scope.UpdateProductOnUI = function (projectInfo, status) {

        switch (status) {
            case 2: {

                var index = $scope.ordersInQueue.indexOf(projectInfo);

                if (index > -1) {

                    $scope.ordersInQueue.splice(index, 1);
                    $scope.ordersInProduction.push(projectInfo);

                } else {
                    $window.alert('UI error');
                }

                break;
            }
            case 3: {
                var index = $scope.ordersInProduction.indexOf(projectInfo);

                if (index > -1) {
                    $scope.ordersInProduction.splice(index, 1);
                    $scope.deliveredPrintObjects.push(projectInfo);
                } else {
                    $window.alert('UI error');
                }

                break;
            }
            default: {
                $window.alert('UI error');
                break;
            }
        }

    }

    $scope.showUploadProductModal = function () {
        UploadPrinObject.showUpload();
    }

    $scope.download = function (printObjectId) {
        UploadPrinObject.download(printObjectId);
    }
}