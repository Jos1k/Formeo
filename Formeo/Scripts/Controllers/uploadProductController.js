var uploadProductController = function ($scope, $modalInstance, $window, $http) {
    $scope.artNo = "";
    $scope.productName = "";
    //$scope.fileName = "";
    $scope.products = [];
    $scope.cancel = function () {
        $modalInstance.dismiss('cancel');
    }
    $scope.addProduct = function () {
        if (!$scope.isBlank($scope.artNo) && !$scope.isBlank($scope.productName)){
            var product = {
                ArtNo: $scope.artNo,
                ProductName: $scope.productName
            };
            $scope.products.push(product);
            $scope.artNo = "";
            $scope.productName = "";
            $scope.fileName = "";
        }
    }
    $scope.removeProduct = function (productIndex) {
        $scope.products.splice(productIndex, 1);
    }

    $scope.isBlank =  function (str) {
        return (str.length === 0 || !str.trim());
    }
}