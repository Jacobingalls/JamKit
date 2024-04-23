using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace info.jacobingalls.jamkit
{
    public class ToStringPubSubKVListener : BasePubSubKVListener<string>
    {

        public override string cast(PubSubKVListenerDelegateRow<string> sub, object value)
        {
            return value.ToString();
        }

        public override object icast(PubSubKVListenerDelegateRow<string> sub, string value)
        {
            throw new InvalidOperationException("Can't cast back to orignal type since we don't know what type that is!");
        }
    }
}
