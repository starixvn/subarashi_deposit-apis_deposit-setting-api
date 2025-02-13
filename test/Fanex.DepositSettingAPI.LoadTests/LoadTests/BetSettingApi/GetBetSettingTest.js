import { sleep } from "k6";
import http from "k6/http";
import { Counter } from "k6/metrics";
import { Env, BaseUrlByEnv, ApiKeyByEnv } from "./Configuration.js"

export const CounterErrors = new Counter('errors');

export const options = {
    // need to config stages before starting
    //stages: [
    //    { duration: "1s", target: 2 },
    //    { duration: "8s", target: 2 },
    //    { duration: "1s", target: 0 },
    //],
    scenarios: {
        constant_request_rate: {
            executor: 'constant-arrival-rate',
            rate: 1000,
            timeUnit: '1s', // 1000 iterations per second, i.e. 1000 RPS
            duration: '30s',
            preAllocatedVUs: 100, // how large the initial pool of VUs would be
            maxVUs: 200, // if the preAllocatedVUs are not enough, we can initialize more
        },
    },
    thresholds: {
        errors: ["count<1"],
    },
    noConnectionReuse: true,
};

export default function () {
    sendRequest(Env.LOCAL);
    sleep(1);
}

const GetRandomAgentId = () => {
    var agentList = [36649189];
    return agentList[Math.floor(Math.random() * agentList.length)];
}

const sendRequest = (env) => {
    const agentId = GetRandomAgentId();

    const url = BaseUrlByEnv[env] + `/api/deposit/betsettings?agentId=${agentId}`;

    const params = {
        headers: {
            "Content-Type": "application/json",
            "ApiKey": ApiKeyByEnv[env],
        },
    };

    const res = http.get(url, params);
    const data = res.json();
    const isSuccess = res.status === 200 && data.errorCode === 0 && data.result !== null;

    CounterErrors.add(!isSuccess);
}

// macOs: K6_WEB_DASHBOARD=true K6_WEB_DASHBOARD_EXPORT="GetBetSettingTest.html" k6 run GetBetSettingTest.js
// windows: $env:K6_WEB_DASHBOARD="true" ; $env:K6_WEB_DASHBOARD_EXPORT="GetBetSettingTest.html" ; k6 run GetBetSettingTest.js
