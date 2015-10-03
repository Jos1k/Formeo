var layOrderPartialController = function ($scope, $modalInstance, $window, $modal, $http) {

    $scope.asd = "Hi";

    $scope.cancel = function () {
        $modalInstance.dismiss('cancel');
    };

    $scope.increasePrintObjectQuantity = function (printObject) {
        printObject.Quantity += 1;
    }

    $scope.decreasePrintObjectQuantity = function (printObject) {
        if (printObject.Quantity - 1 > 0) {
            printObject.Quantity -= 1;
        }
    }

    $scope.removePrintObjectFromOrder = function (printObject) {
        var index = $scope.printObjectsInfoModal.indexOf(printObject);

        if (index > -1) {
            $scope.printObjectsInfoModal.splice(index, 1);
        }
    }

    $scope.LayOrder = function () {

        $http({
            method: 'GET',
            url: '/Project/LayOrderConfirm',
            params: { 'selectedPrintObjectIds': $scope.selectedPrintObjectIds },
            headers: { 'Content-Type': 'application/x-www-form-urlencoded' }
        }).
              then(function (response) {
                  //success
                  var modalInstance = $modal.open({
                      template: (response.data),
                      controller: 'layOrderConfirmPartialController',
                      backdrop: 'static',
                      resolve: {
                          printObjectsInfoModal: function () { return $scope.printObjectsInfoModal; },
                          deliveryInfo: function () { return $scope.deliveryInfo; }
                      }
                  });
                  $scope.layOrderButtonIsDisabled = false;


              }, function (response) {
                  //error
                  $window.alert('error');
                  $scope.layOrderButtonIsDisabled = false;

              });
    }

}
