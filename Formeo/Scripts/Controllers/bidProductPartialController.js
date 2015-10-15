var bidProductPartialController = function ($scope, $modalInstance, $window, $http, UploadPrinObject) {

    $scope.asd = "Hi";
    $scope.price = 0.01;
    $scope.printObjectInfoModal = "";

    $scope.cancel = function () {
        $modalInstance.dismiss('cancel');
    };
    $scope.download = function (printObjectId) {
        UploadPrinObject.download(printObjectId);
    };
    $scope.BidProduct = function () {
        $http({
            method: 'POST',
            url: '/Bids/CreateBid',
            params: { 'printObjectId': $scope.printObjectInfoModal.Id, 'price': $scope.price },
            headers: { 'Content-Type': 'application/json;' },
            data: ''
        }).
           then(function (response) {
               $modalInstance.close(response.data);
           }, function (response) {
               $window.alert('error creating bid');
               $modalInstance.dismiss('cancel');
           });
    };

}