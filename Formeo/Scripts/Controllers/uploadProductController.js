var uploadProductController = function ( $scope ,Upload, $timeout,$modalInstance, $window, $http) {
    $scope.isUploadInProgress = false;
    $scope.artNo;
    $scope.productName = "";
    $picFile = null;
    //$scope.fileName = "";
    $scope.products = [];
    $scope.cancel = function () {
        $scope.products = null;
        $modalInstance.dismiss('cancel');
    }
    $scope.addProduct = function (file) {
        if ($scope.artNo && !$scope.isBlank($scope.productName) && file != null && !$scope.isBlank($scope.picFile.name)) {
            var product = {
                artNo: $scope.artNo,
                productName: $scope.productName,
                file: file
            };
            $scope.products.push(product);
            $scope.artNo = "";
            $scope.productName = "";
            $scope.fileName = "";
            $scope.picFile = null;
        }
    }

    $scope.upload = function () {
        if ($scope.products && $scope.products.length) {
            for (var i = 0; i < $scope.products.length; i++) {
                var product = $scope.products[i];
                if (!product.$error) {
                    Upload.upload({
                        url: '/Home/UploadProduct',
                        data: {
                            artNo: product.artNo,
                            productName: product.productName,
                            file: product.file
                        }
                    })
                        .progress(function (evt) {
                            $scope.isUploadInProgress = true;
                    })
                        .success(function (data, status, headers, config) {
                            $scope.isUploadInProgress = false;
                            //alert("Uploading finished!");
                            $modalInstance.dismiss('cancel');
                    });
                }
            }
            $scope.isUploadInProgress = false;
        }
    }


    $scope.removeProduct = function (productIndex) {
        $scope.products.splice(productIndex, 1);
    }

    $scope.isBlank =  function (str) {
        return (str.length === 0 || !str.trim());
    }
}