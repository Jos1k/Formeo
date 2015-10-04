var addProductsPartialController = function ($scope, $modalInstance, $window, $http, parentPrintObjectsInfoModal) {

    $scope.asd = "Hi";
    $scope.parentPrintObjectsInfoModal = parentPrintObjectsInfoModal;
    $scope.selectedPrintObjectsInfoModal = [];

   
    $scope.cancel = function () {
        $modalInstance.dismiss('cancel');
    };

    $scope.Add = function () {

        angular.forEach($scope.selectedPrintObjectsInfoModal, function (index) {
            index.Quantity = 1;//hack
            $scope.parentPrintObjectsInfoModal.push(index);
        });
        $modalInstance.dismiss('cancel');
    };

    $scope.printObjectIsSelected = function (printObject) {
        return $scope.selectedPrintObjectsInfoModal.indexOf(printObject) > -1;
    }

    $scope.addOrRemovePrintobject = function (printObject) {
        var index = $scope.selectedPrintObjectsInfoModal.indexOf(printObject);

        if (index > -1) {
            $scope.selectedPrintObjectsInfoModal.splice(index, 1);
        }
        else {
            $scope.selectedPrintObjectsInfoModal.push(printObject);
        }
    }

}