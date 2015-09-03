/// <reference path="../app.ts" />

module app.services {

    export class OrderService implements IService {
        http;

        constructor($http: ng.IHttpService)
        {
            this.http = $http;
        }

        addItemToCart = (itemId) =>
        {
            // Store the id of the item in case we are not authenticated yet
            sessionStorage.setItem("tempItemId", itemId);
            this.http.post('/api/store/cart', { itemId: itemId }).then(() =>
            {
            });
        };
    };
}

angular.module('app').factory('orderService', ['$http', ($http) => new app.services.OrderService($http)]);
//app.registerService('AuthService', ['$http', '$q']);