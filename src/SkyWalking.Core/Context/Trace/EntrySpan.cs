﻿/*
 * Licensed to the OpenSkywalking under one or more
 * contributor license agreements.  See the NOTICE file distributed with
 * this work for additional information regarding copyright ownership.
 * The ASF licenses this file to You under the Apache License, Version 2.0
 * (the "License"); you may not use this file except in compliance with
 * the License.  You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 *
 */

using SkyWalking.Components;

namespace SkyWalking.Context.Trace
{
    public class EntrySpan : StackBasedTracingSpan
    {
        private int _currentMaxDepth;

        public EntrySpan(int spanId, int parentSpanId, string operationName)
            : base(spanId, parentSpanId, operationName)
        {
            _stackDepth = 0;
        }

        public EntrySpan(int spanId, int parentSpanId, int operationId)
            : base(spanId, parentSpanId, operationId)
        {
            _stackDepth = 0;
        }

        public override bool IsEntry => true;

        public override bool IsExit => false;

        public override ISpan Start()
        {
            if ((_currentMaxDepth = ++_stackDepth) == 1)
            {
                base.Start();
            }
            ClearWhenRestart();
            return this;
        }

        public override ISpan Tag(string key, string value)
        {
            if (_stackDepth == _currentMaxDepth)
            {
                base.Tag(key, value);
            }
            return this;
        }

        public override ISpan SetLayer(SpanLayer layer)
        {
            if (_stackDepth == _currentMaxDepth)
            {
                return base.SetLayer(layer);
            }
            return this;
        }

        public override ISpan SetComponent(IComponent component)
        {
            if (_stackDepth == _currentMaxDepth)
            {
                return base.SetComponent(component);
            }
            return this;
        }

        public override ISpan SetComponent(string componentName)
        {
            if (_stackDepth == _currentMaxDepth)
            {
                return base.SetComponent(componentName);
            }
            return this;
        }

        public override string OperationName
        {
            get
            {
                return base.OperationName;
            }
            set
            {
                if (_stackDepth == _currentMaxDepth)
                {
                    base.OperationName = value;
                }
            }
        }

        public override int OperationId
        {
            get
            {
                return base.OperationId;
            }
            set
            {
                if (_stackDepth == _currentMaxDepth)
                {
                    base.OperationId = value;
                }
            }
        }

        private void ClearWhenRestart()
        {
            _componentId = 0;
            _componentName = null;
            _layer = null;
            _logs = null;
            _tags = null;
        }
    }
}
