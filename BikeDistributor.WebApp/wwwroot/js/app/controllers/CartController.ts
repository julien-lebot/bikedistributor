/// <reference path="../app.ts" />

module app.controllers {
    export class CartController implements IController {
        constructor($scope, $http: ng.IHttpService) {
            $http.get('/api/cart').
                then(response => {

                },
                response => {
                    // handle response
                });
        }
    };
}

app.registerController('CartController', ['$scope', '$http']);