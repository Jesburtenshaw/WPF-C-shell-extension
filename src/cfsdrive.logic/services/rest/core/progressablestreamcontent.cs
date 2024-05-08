#region Copyright (c) 2021
/* Tierfive Explorer Extension
 * Copyright (c) 2021
 * 
 * Company	: Tierfive
 * 
 * File     : progressablestreamcontent.cs 
 * 
 * Contents	: The calss allows to use progress bar with http content
 * 
 * Author	: Sergey Fasonov
 * 
 */
#endregion

using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace cfsdrive.logic.services.rest.core
{
    internal class StreamContentWithProgress : HttpContent
    {
        /// <summary>
        /// Lets keep buffer of 20kb
        /// </summary>
        private const int DefaultBufferSize = 16384;

        private readonly HttpContent _content;
        private readonly int _bufferSize;
        private readonly Action<long, long> _progressCallback;

        public StreamContentWithProgress(HttpContent content, Action<long, long> progressCallback) 
            : this(content,
            DefaultBufferSize, progressCallback)
        {
        }

        public StreamContentWithProgress(HttpContent content, int bufferSize, Action<long, long> progressCallback)
        {
            if (content == null)
            {
                throw new ArgumentNullException("content");
            }

            if (bufferSize <= 0)
            {
                throw new ArgumentOutOfRangeException("bufferSize");
            }

            _content = content;
            _bufferSize = bufferSize;
            _progressCallback = progressCallback;

            foreach (var h in content.Headers)
            {
                this.Headers.Add(h.Key, h.Value);
            }
        }

        protected override Task SerializeToStreamAsync(Stream stream, TransportContext context)
        {
            return Task.Run(async () =>
            {
                var buffer = new byte[_bufferSize];
                TryComputeLength(out long size);
                long uploaded = 0;

                using (var sinput = await _content.ReadAsStreamAsync())
                {
                    while (true)
                    {
                        int length = sinput.Read(buffer, 0, buffer.Length);
                        if (length <= 0) 
                            break;

                        uploaded += length;
                        _progressCallback?.Invoke(uploaded, size);

                        stream.Write(buffer, 0, length);
                        stream.Flush();
                    }
                }

                stream.Flush();
            });
        }

        protected override bool TryComputeLength(out long length)
        {
            length = _content.Headers.ContentLength.GetValueOrDefault();
            return true;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _content.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}
