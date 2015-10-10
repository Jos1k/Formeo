var bidProductPartialController = function ($scope, $modalInstance, $window, $http) {

    $scope.asd = "Hi";
    $scope.price=0.01;

    $scope.cancel = function () {
        $modalInstance.dismiss('cancel');
    };

    $scope.BidProduct = function () {
        $http({
            method: 'POST',
            url: '/Project/CreateOrder',
            params: { 'orderName': $scope.orderName, 'printObjectInfo': JSON.stringify(infosParam), 'deliveryInfo': $scope.deliveryInfo },
            headers: { 'Content-Type': 'application/json;' },
            data: ''
        }).
           then(function (response) {
              
           }, function (response) {
             
           });
    };

}