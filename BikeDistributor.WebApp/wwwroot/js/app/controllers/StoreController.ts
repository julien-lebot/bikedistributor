/// <reference path="../app.ts" />

module app.controllers {
    export class StoreController implements IController {

        constructor($scope, $http: ng.IHttpService, $location: ng.ILocationService, orderService)
        {
            $http.get('/api/store').
                then(response => {
                    $scope.bikes = response.data;
                },
                response => {
                    // handle response
                });
            $scope.addToCart = (id) =>
            {
                orderService.addItemToCart(id);
                $location.path('/cart');
            };
        }
    };
}

app.registerController('StoreController', ['$scope', '$http', '$location', 'orderService']);