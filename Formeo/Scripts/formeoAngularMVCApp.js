var formeoAngularMVCApp = angular.module('formeoAngularMVCApp', ['ngFileUpload', 'ui.bootstrap']);

formeoAngularMVCApp.controller('commonController', commonController);
formeoAngularMVCApp.controller('adminPageController', adminPageController);
formeoAngularMVCApp.controller('customerPageController', customerPageController);
formeoAngularMVCApp.controller('producerPageController', producerPageController);

formeoAngularMVCApp.controller('layOrderPartialController', layOrderPartialController);
formeoAngularMVCApp.controller('layOrderConfirmPartialController', layOrderConfirmPartialController);
formeoAngularMVCApp.controller('addProductsPartialController', addProductsPartialController);
formeoAngularMVCApp.controller('bidsForPrintObjectController', bidsForPrintObjectController);formeoAngularMVCApp.controller('uploadProductController', uploadProductController);