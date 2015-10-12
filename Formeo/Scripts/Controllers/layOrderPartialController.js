﻿var layOrderPartialController = function ($scope, $modalInstance, $window, $modal, $http) {

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
            headers: { 'Content-Type': 'application/x-www-form-urlencoded' }
        }).
              then(function (response) {
                  //success
                  var modalInstance = $modal.open({
                      template: (response.data),
                      controller: 'layOrderConfirmPartialController',
                      backdrop: 'static',
                      resolve: {
                          orderName: function () { return $scope.orderName; },
                          printObjectsInfoModal: function () { return $scope.printObjectsInfoModal; },
                          deliveryInfo: function () { return $scope.deliveryInfo; }
                      }
                  });
                  modalInstance.result.then(function (response) {
                      $modalInstance.close(response);
                  }, function (response) {
                      //error
                      $window.alert('error 123');
                      $modalInstance.dismiss('cancel');

                  });


              }, function (response) {
                  //error
                  $window.alert('error');

              });
    }

    $scope.AddProduct = function () {

        var pObjectIds = []

        angular.forEach($scope.printObjectsInfoModal, function (index) {
            pObjectIds.push(index.Id);
        });

        $http({
            method: 'GET',
            url: '/Project/AddProducts',
            params: { 'selectedPrintObjectIds': JSON.stringify(pObjectIds) },
            headers: { 'Content-Type': 'application/json' },
            data: ''
        }).
              then(function (response) {
                  //success
                  var modalInstance = $modal.open({
                      template: (response.data),
                      controller: 'addProductsPartialController',
                      backdrop: 'static',
                      resolve: {
                          parentPrintObjectsInfoModal: function () { return $scope.printObjectsInfoModal; },
                      }
                  });


              }, function (response) {
                  //error
                  $window.alert('error');

              });
    }

}
