﻿var uploadProductController = function ($scope, Upload, $timeout, $modalInstance, $window, $http) {
    $scope.isUploadInProgress = false;
    $scope.artNo;
    $scope.printMaterial = "";
    $scope.productName = "";
    $picFile = null;
    //$scope.fileName = "";
    $scope.products = [];
    $scope.cancel = function () {
        $scope.products = null;
        $modalInstance.dismiss('cancel');
    };
    $scope.addProduct = function (file) {
        if (!$scope.isBlank($scope.printMaterial) && $scope.artNo && !$scope.isBlank($scope.productName) && file != null && !$scope.isBlank($scope.picFile.name)) {
            var product = {
                artNo: $scope.artNo,
                printMaterial: $scope.printMaterial,
                productName: $scope.productName,
                file: file
            };
            $scope.products.push(product);
            $scope.artNo = "";
            $scope.printMaterial = "";
            $scope.productName = "";
            $scope.fileName = "";
            $scope.picFile = null;
        }
    };
    $scope.upload = function () {
        if ($scope.products && $scope.products.length) {
            var uploadListProducts = [];
            var uploadListFiles = [];
            for (var i = 0; i < $scope.products.length; i++) {
                var product = $scope.products[i];
                if (!product.$error) {
                    var uploadProduct = {
                        artNo: product.artNo,
                        printMaterial: product.printMaterial,
                        productName: product.productName
                    };
                    var uploadFile = product.file;

                    uploadListProducts.push(uploadProduct);
                    uploadListFiles.push(uploadFile);
                }
            }

            Upload.upload({
                url: '/PrintObjects/UploadProducts',
                data: {
                    products: uploadListProducts,
                    files: uploadListFiles
                }
            })
                .progress(function (evt) {
                    $scope.isUploadInProgress = true;
                })
                .success(function (data, status, headers, config) {
                    $scope.isUploadInProgress = false;
                    alert("Uploading finished!");
                    $modalInstance.close(data);
                });
        }
    };
    $scope.removeProduct = function (productIndex) {
        $scope.products.splice(productIndex, 1);
    };
    $scope.isBlank = function (str) {
        return (str.length === 0 || !str.trim());
    };
}