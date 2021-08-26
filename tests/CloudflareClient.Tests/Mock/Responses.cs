namespace CloudflareClient.Tests.Mock
{
  public static class Responses
  {
    public const string LoadBalancerMonitorDetails = @"{
  ""success"": true,
    ""errors"": [],
    ""messages"": [],
    ""result"": {
      ""id"": ""f1aba936b94213e5b8dca0c0dbf1f9cc"",
      ""created_on"": ""2014-01-01T05:20:00.12345Z"",
      ""modified_on"": ""2014-01-01T05:20:00.12345Z"",
      ""type"": ""https"",
      ""description"": ""Login page monitor"",
      ""method"": ""GET"",
      ""path"": ""/health"",
      ""header"": {
        ""Host"": [
        ""example.com""
          ],
        ""X-App-ID"": [
        ""abc123""
          ]
      },
      ""port"": 8080,
      ""timeout"": 3,
      ""retries"": 0,
      ""interval"": 90,
      ""expected_body"": ""alive"",
      ""expected_codes"": ""2xx"",
      ""follow_redirects"": true,
      ""allow_insecure"": true
    }
  }";
    public const string ListLoadBalancerMonitors = @"{
  ""success"": true,
    ""errors"": [],
    ""messages"": [],
    ""result"": [
    {
      ""id"": ""f1aba936b94213e5b8dca0c0dbf1f9cc"",
      ""created_on"": ""2014-01-01T05:20:00.12345Z"",
      ""modified_on"": ""2014-01-01T05:20:00.12345Z"",
      ""type"": ""https"",
      ""description"": ""Login page monitor"",
      ""method"": ""GET"",
      ""path"": ""/health"",
      ""header"": {
        ""Host"": [
        ""example.com""
          ],
        ""X-App-ID"": [
        ""abc123""
          ]
      },
      ""port"": 8080,
      ""timeout"": 3,
      ""retries"": 0,
      ""interval"": 90,
      ""expected_body"": ""alive"",
      ""expected_codes"": ""2xx"",
      ""follow_redirects"": true,
      ""allow_insecure"": true
    }
    ]
  }";
    public const string ListLoadBalancerPools = @"{
  ""success"": true,
    ""errors"": [],
    ""messages"": [],
    ""result"": [
    {
      ""id"": ""17b5962d775c646f3f9725cbc7a53df4"",
      ""created_on"": ""2014-01-01T05:20:00.12345Z"",
      ""modified_on"": ""2014-01-01T05:20:00.12345Z"",
      ""description"": ""Primary data center - Provider XYZ"",
      ""name"": ""primary-dc-1"",
      ""enabled"": false,
      ""load_shedding"": {
        ""default_percent"": 0,
        ""default_policy"": ""random"",
        ""session_percent"": 0,
        ""session_policy"": ""hash""
      },
      ""minimum_origins"": 2,
      ""monitor"": ""f1aba936b94213e5b8dca0c0dbf1f9cc"",
      ""check_regions"": [
      ""WEU"",
      ""ENAM""
        ],
      ""origins"": [
      {
        ""name"": ""app-server-1"",
        ""address"": ""0.0.0.0"",
        ""enabled"": true,
        ""weight"": 0.5,
        ""header"": {
          ""Host"": [
          ""example1.com""
            ]
        }
      },{
        ""name"": ""app-server-2"",
        ""address"": ""0.0.0.1"",
        ""enabled"": true,
        ""weight"": 0.5
      }
      ],
      ""notification_email"": ""someone@example.com,sometwo@example.com"",
      ""notification_filter"": {
        ""origin"": {
          ""disable"": false,
          ""healthy"": null
        },
        ""pool"": {
          ""disable"": false,
          ""healthy"": null
        }
      }
    }
    ]
  }";

    public const string LoadBalancerPoolDetails = @"{
  ""success"": true,
    ""errors"": [],
    ""messages"": [],
    ""result"": {
      ""id"": ""17b5962d775c646f3f9725cbc7a53df4"",
      ""created_on"": ""2014-01-01T05:20:00.12345Z"",
      ""modified_on"": ""2014-01-01T05:20:00.12345Z"",
      ""description"": ""Primary data center - Provider XYZ"",
      ""name"": ""primary-dc-1"",
      ""enabled"": false,
      ""load_shedding"": {
        ""default_percent"": 0,
        ""default_policy"": ""random"",
        ""session_percent"": 0,
        ""session_policy"": ""hash""
      },
      ""minimum_origins"": 2,
      ""monitor"": ""f1aba936b94213e5b8dca0c0dbf1f9cc"",
      ""check_regions"": [
      ""WEU"",
      ""ENAM""
        ],
      ""origins"": [
      {
        ""name"": ""app-server-1"",
        ""address"": ""127.0.0.1"",
        ""enabled"": true,
        ""weight"": 0.56,
        ""header"": {
          ""Host"": [
          ""example.com""
            ]
        }
      },{
        ""name"": ""app-server-2"",
        ""address"": ""0.0.0.1"",
        ""enabled"": true,
        ""weight"": 0.5
      }
      ],
      ""notification_email"": ""someone@example.com,sometwo@example.com"",
      ""notification_filter"": {
        ""origin"": {
          ""disable"": false,
          ""healthy"": null
        },
        ""pool"": {
          ""disable"": false,
          ""healthy"": null
        }
      }
    }
  }";

    public const string LoadBalancerDetails = @"{
  ""success"": true,
  ""errors"": [],
  ""messages"": [],
  ""result"": {
    ""id"": ""699d98642c564d2e855e9661899b7252"",
    ""created_on"": ""2014-01-01T05:20:00.12345Z"",
    ""modified_on"": ""2014-01-01T05:20:00.12345Z"",
    ""description"": ""Load Balancer for www.example.com"",
    ""name"": ""www.example.com"",
    ""enabled"": true,
    ""ttl"": 30,
    ""fallback_pool"": ""17b5962d775c646f3f9725cbc7a53df4"",
    ""default_pools"": [
      ""17b5962d775c646f3f9725cbc7a53df4"",
      ""9290f38c5d07c2e2f4df57b1f61d4196"",
      ""00920f38ce07c2e2f4df50b1f61d4194""
    ],
    ""region_pools"": {
      ""WNAM"": [
        ""de90f38ced07c2e2f4df50b1f61d4194"",
        ""9290f38c5d07c2e2f4df57b1f61d4196""
      ],
      ""ENAM"": [
        ""00920f38ce07c2e2f4df50b1f61d4194""
      ]
    },
    ""pop_pools"": {
      ""LAX"": [
        ""de90f38ced07c2e2f4df50b1f61d4194"",
        ""9290f38c5d07c2e2f4df57b1f61d4196""
      ],
      ""LHR"": [
        ""abd90f38ced07c2e2f4df50b1f61d4194"",
        ""f9138c5d07c2e2f4df57b1f61d4196""
      ],
      ""SJC"": [
        ""00920f38ce07c2e2f4df50b1f61d4194""
      ]
    },
    ""proxied"": true,
    ""steering_policy"": ""dynamic_latency"",
    ""session_affinity"": ""cookie"",
    ""session_affinity_attributes"": {
      ""samesite"": ""Auto"",
      ""secure"": ""Auto"",
      ""drain_duration"": 100
    },
    ""session_affinity_ttl"": 5000,
    ""rules"": [
      {
        ""name"": ""route the path /testing to testing datacenter."",
        ""condition"": ""http.request.uri.path contains \""/testing\"""",
        ""overrides"": {
          ""session_affinity"": ""cookie"",
          ""session_affinity_ttl"": 5000,
          ""session_affinity_attributes"": {
            ""samesite"": ""Auto"",
            ""secure"": ""Auto"",
            ""drain_duration"": 100
          },
          ""ttl"": 30,
          ""steering_policy"": ""dynamic_latency"",
          ""fallback_pool"": ""17b5962d775c646f3f9725cbc7a53df4"",
          ""default_pools"": [
            ""17b5962d775c646f3f9725cbc7a53df4"",
            ""9290f38c5d07c2e2f4df57b1f61d4196"",
            ""00920f38ce07c2e2f4df50b1f61d4194""
          ],
          ""pop_pools"": {
            ""LAX"": [
              ""de90f38ced07c2e2f4df50b1f61d4194"",
              ""9290f38c5d07c2e2f4df57b1f61d4196""
            ],
            ""LHR"": [
              ""abd90f38ced07c2e2f4df50b1f61d4194"",
              ""f9138c5d07c2e2f4df57b1f61d4196""
            ],
            ""SJC"": [
              ""00920f38ce07c2e2f4df50b1f61d4194""
            ]
          },
          ""region_pools"": {
            ""WNAM"": [
              ""de90f38ced07c2e2f4df50b1f61d4194"",
              ""9290f38c5d07c2e2f4df57b1f61d4196""
            ],
            ""ENAM"": [
              ""00920f38ce07c2e2f4df50b1f61d4194""
            ]
          }
        },
        ""priority"": 55,
        ""disabled"": false,
        ""terminates"": false,
        ""fixed_response"": {
          ""message_body"": ""Testing Hello"",
          ""status_code"": 200,
          ""content_type"": ""application/json"",
          ""location"": ""www.example.com""
        }
      }
    ]
  }
}";

    public const string ListLoadBalancers = @"{
  ""success"": true,
  ""errors"": [],
  ""messages"": [],
  ""result"": [
    {
      ""id"": ""699d98642c564d2e855e9661899b7252"",
      ""created_on"": ""2014-01-01T05:20:00.12345Z"",
      ""modified_on"": ""2014-01-01T05:20:00.12345Z"",
      ""description"": ""Load Balancer for www.example.com"",
      ""name"": ""www.example.com"",
      ""enabled"": true,
      ""ttl"": 30,
      ""fallback_pool"": ""17b5962d775c646f3f9725cbc7a53df4"",
      ""default_pools"": [
        ""17b5962d775c646f3f9725cbc7a53df4"",
        ""9290f38c5d07c2e2f4df57b1f61d4196"",
        ""00920f38ce07c2e2f4df50b1f61d4194""
      ],
      ""region_pools"": {
        ""WNAM"": [
          ""de90f38ced07c2e2f4df50b1f61d4194"",
          ""9290f38c5d07c2e2f4df57b1f61d4196""
        ],
        ""ENAM"": [
          ""00920f38ce07c2e2f4df50b1f61d4194""
        ]
      },
      ""pop_pools"": {
        ""LAX"": [
          ""de90f38ced07c2e2f4df50b1f61d4194"",
          ""9290f38c5d07c2e2f4df57b1f61d4196""
        ],
        ""LHR"": [
          ""abd90f38ced07c2e2f4df50b1f61d4194"",
          ""f9138c5d07c2e2f4df57b1f61d4196""
        ],
        ""SJC"": [
          ""00920f38ce07c2e2f4df50b1f61d4194""
        ]
      },
      ""proxied"": true,
      ""steering_policy"": ""dynamic_latency"",
      ""session_affinity"": ""cookie"",
      ""session_affinity_attributes"": {
        ""samesite"": ""Auto"",
        ""secure"": ""Auto"",
        ""drain_duration"": 100
      },
      ""session_affinity_ttl"": 5000,
      ""rules"": [
        {
          ""name"": ""route the path /testing to testing datacenter."",
          ""condition"": ""http.request.uri.path contains \""/testing\"""",
          ""overrides"": {
            ""session_affinity"": ""cookie"",
            ""session_affinity_ttl"": 5000,
            ""session_affinity_attributes"": {
              ""samesite"": ""Auto"",
              ""secure"": ""Auto"",
              ""drain_duration"": 100
            },
            ""ttl"": 30,
            ""steering_policy"": ""dynamic_latency"",
            ""fallback_pool"": ""17b5962d775c646f3f9725cbc7a53df4"",
            ""default_pools"": [
              ""17b5962d775c646f3f9725cbc7a53df4"",
              ""9290f38c5d07c2e2f4df57b1f61d4196"",
              ""00920f38ce07c2e2f4df50b1f61d4194""
            ],
            ""pop_pools"": {
              ""LAX"": [
                ""de90f38ced07c2e2f4df50b1f61d4194"",
                ""9290f38c5d07c2e2f4df57b1f61d4196""
              ],
              ""LHR"": [
                ""abd90f38ced07c2e2f4df50b1f61d4194"",
                ""f9138c5d07c2e2f4df57b1f61d4196""
              ],
              ""SJC"": [
                ""00920f38ce07c2e2f4df50b1f61d4194""
              ]
            },
            ""region_pools"": {
              ""WNAM"": [
                ""de90f38ced07c2e2f4df50b1f61d4194"",
                ""9290f38c5d07c2e2f4df57b1f61d4196""
              ],
              ""ENAM"": [
                ""00920f38ce07c2e2f4df50b1f61d4194""
              ]
            }
          },
          ""priority"": 55,
          ""disabled"": false,
          ""terminates"": false,
          ""fixed_response"": {
            ""message_body"": ""Testing Hello"",
            ""status_code"": 200,
            ""content_type"": ""application/json"",
            ""location"": ""www.example.com""
          }
        }
      ]
    }
  ]
}";

    public const string UserDetails = @"{
          ""success"": true,
          ""errors"": [],
          ""messages"": [],
          ""result"": {
            ""id"": ""7c5dae5552338874e5053f2534d2767a"",
            ""email"": ""user@example.com"",
            ""first_name"": ""John"",
            ""last_name"": ""Appleseed"",
            ""username"": ""cfuser12345"",
            ""telephone"": ""+1 123-123-1234"",
            ""country"": ""US"",
            ""zipcode"": ""12345"",
            ""created_on"": ""2014-01-01T05:20:00Z"",
            ""modified_on"": ""2014-01-01T05:20:00Z"",
            ""two_factor_authentication_enabled"": false,
            ""suspended"": false
          }
        }";

    public const string EditUser = @"{
        ""success"": true,
        ""errors"": [],
        ""messages"": [],
        ""result"": {
          ""id"": ""7c5dae5552338874e5053f2534d2767a"",
          ""email"": ""user@example.com"",
          ""first_name"": ""John"",
          ""last_name"": ""Appleseed"",
          ""username"": ""cfuser12345"",
          ""telephone"": ""+1 123-123-1234"",
          ""country"": ""US"",
          ""zipcode"": ""12345"",
          ""created_on"": ""2014-01-01T05:20:00Z"",
          ""modified_on"": ""2014-01-01T05:20:00Z"",
          ""two_factor_authentication_enabled"": false,
          ""suspended"": false
        }
      }";

    public const string ZoneDetails = @"{
  ""success"": true,
    ""errors"": [],
    ""messages"": [],
    ""result"": {
      ""id"": ""023e105f4ecef8ad9ca31a8372d0c353"",
      ""name"": ""example.com"",
      ""development_mode"": 7200,
      ""original_name_servers"": [
      ""ns1.originaldnshost.com"",
      ""ns2.originaldnshost.com""
        ],
      ""original_registrar"": ""GoDaddy"",
      ""original_dnshost"": ""NameCheap"",
      ""created_on"": ""2014-01-01T05:20:00.12345Z"",
      ""modified_on"": ""2014-01-01T05:20:00.12345Z"",
      ""activated_on"": ""2014-01-02T00:01:00.12345Z"",
      ""owner"": {
        ""id"": ""1234"",
        ""email"": ""some@email.com"",
        ""type"": ""user""
      },
      ""account"": {
        ""id"": ""01a7362d577a6c3019a474fd6f485823"",
        ""name"": ""Demo Account""
      },
      ""permissions"": [
      ""#zone:read"",
      ""#zone:edit""
        ],
      ""plan"": {
        ""id"": ""e592fd9519420ba7405e1307bff33214"",
        ""name"": ""Pro Plan"",
        ""price"": 20,
        ""currency"": ""USD"",
        ""frequency"": ""monthly"",
        ""legacy_id"": ""pro"",
        ""is_subscribed"": true,
        ""can_subscribe"": true
      },
      ""plan_pending"": {
        ""id"": ""e592fd9519420ba7405e1307bff33214"",
        ""name"": ""Pro Plan"",
        ""price"": 20,
        ""currency"": ""USD"",
        ""frequency"": ""monthly"",
        ""legacy_id"": ""pro"",
        ""is_subscribed"": true,
        ""can_subscribe"": true
      },
      ""status"": ""active"",
      ""paused"": false,
      ""type"": ""full"",
      ""name_servers"": [
      ""tony.ns.cloudflare.com"",
      ""woz.ns.cloudflare.com""
        ]
    }
  }";

    public const string ListZones = @"{
      ""success"": true,
      ""errors"": [],
      ""messages"": [],
      ""result"": [
      {
        ""id"": ""023e105f4ecef8ad9ca31a8372d0c353"",
        ""name"": ""example.com"",
        ""development_mode"": 7200,
        ""original_name_servers"": [
          ""ns1.originaldnshost.com"",
          ""ns2.originaldnshost.com""
        ],
        ""original_registrar"": ""GoDaddy"",
        ""original_dnshost"": ""NameCheap"",
        ""created_on"": ""2014-01-01T05:20:00.12345Z"",
        ""modified_on"": ""2014-01-01T05:20:00.12345Z"",
        ""activated_on"": ""2014-01-02T00:01:00.12345Z"",
        ""owner"": {
          ""id"": ""1234"",
          ""email"": ""some@email.com"",
          ""type"": ""user""
        },
        ""account"": {
          ""id"": ""01a7362d577a6c3019a474fd6f485823"",
          ""name"": ""Demo Account""
        },
        ""permissions"": [
          ""#zone:read"",
          ""#zone:edit""
        ],
        ""plan"": {
          ""id"": ""e592fd9519420ba7405e1307bff33214"",
          ""name"": ""Pro Plan"",
          ""price"": 20,
          ""currency"": ""USD"",
          ""frequency"": ""monthly"",
          ""legacy_id"": ""pro"",
          ""is_subscribed"": true,
          ""can_subscribe"": true
        },
        ""plan_pending"": {
          ""id"": ""e592fd9519420ba7405e1307bff33214"",
          ""name"": ""Pro Plan"",
          ""price"": 20,
          ""currency"": ""USD"",
          ""frequency"": ""monthly"",
          ""legacy_id"": ""pro"",
          ""is_subscribed"": true,
          ""can_subscribe"": true
        },
        ""status"": ""active"",
        ""paused"": false,
        ""type"": ""full"",
        ""name_servers"": [
          ""tony.ns.cloudflare.com"",
          ""woz.ns.cloudflare.com""
        ]
      }
      ]
    }";
  }
}
