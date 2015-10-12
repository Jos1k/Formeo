var commonController = function ($scope, $window, $http, $modal) {
    $scope.showUploadProductModal = function () {
        $http({
            method: 'GET',
            url: '/PrintObjects/UploadProductShowModal',
            //params: { 'selectedPrintObjectIds': $scope.selectedPrintObjectIds },
            headers: { 'Content-Type': 'application/x-www-form-urlencoded' }
        }).
        then(function (response) {
            //success
            var modalInstance = $modal.open({
                template: (response.data),
                controller: 'uploadProductController',
                backdrop: 'static',
                windowClass: 'app-modal-window'
            }); 
        }, function (response) {
            //error
            $window.alert('error');
        });
    }
}