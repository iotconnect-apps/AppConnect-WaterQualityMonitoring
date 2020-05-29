"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var testing_1 = require("@angular/core/testing");
var sensor_service_1 = require("./sensor.service");
describe('SensorService', function () {
    beforeEach(function () { return testing_1.TestBed.configureTestingModule({}); });
    it('should be created', function () {
        var service = testing_1.TestBed.get(sensor_service_1.SensorService);
        expect(service).toBeTruthy();
    });
});
//# sourceMappingURL=sensor.service.spec.js.map