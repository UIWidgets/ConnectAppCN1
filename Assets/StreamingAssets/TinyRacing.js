// Copyright 2010 The Emscripten Authors.  All rights reserved.
// Emscripten is available under two separate licenses, the MIT license and the
// University of Illinois/NCSA Open Source License.  Both these licenses can be
// found in the LICENSE file.

var Module = Module;






// Redefine these in a --pre-js to override behavior. If you would like to
// remove out() or err() altogether, you can no-op it out to function() {},
// and build with --closure 1 to get Closure optimize out all the uses
// altogether.

function out(text) {
  console.log(text);
}

function err(text) {
  console.error(text);
}

// Override this function in a --pre-js file to get a signal for when
// compilation is ready. In that callback, call the function run() to start
// the program.
function ready() {
    run();
}

// --pre-jses are emitted after the Module integration code, so that they can
// refer to Module (if they choose; they can also define Module)

function ready() {
	try {
		if (typeof ENVIRONMENT_IS_PTHREAD === 'undefined' || !ENVIRONMENT_IS_PTHREAD) run();
	} catch(e) {
		// Suppress the JS throw message that corresponds to Dots unwinding the call stack to run the application. 
		if (e !== 'unwind') throw e;
	}
}

(function(global, module){
    var _allocateArrayOnHeap = function (typedArray) {
        var requiredMemorySize = typedArray.length * typedArray.BYTES_PER_ELEMENT;
        var ptr = _malloc(requiredMemorySize);
        var heapBytes = new Uint8Array(HEAPU8.buffer, ptr, requiredMemorySize);
        heapBytes.set(new Uint8Array(typedArray.buffer));
        return heapBytes;
    };
    
    var _allocateStringOnHeap = function (string) {
        var bufferSize = lengthBytesUTF8(string) + 1;
        var ptr = _malloc(bufferSize);
        stringToUTF8(string, ptr, bufferSize);
        return ptr;
    };

    var _freeArrayFromHeap = function (heapBytes) {
        if(typeof heapBytes !== "undefined")
            _free(heapBytes.byteOffset);
    };
    
    var _freeStringFromHeap = function (stringPtr) {
        if(typeof stringPtr !== "undefined")
            _free(stringPtr);
    };

    var _sendMessage = function(message, intArr, floatArr, byteArray) {
        if (!Array.isArray(intArr)) {
            intArr = [];
        }
        if (!Array.isArray(floatArr)) {
            floatArr = [];
        }
        if (!Array.isArray(byteArray)) {
            byteArray = [];
        }
        
        var messageOnHeap, intOnHeap, floatOnHeap, bytesOnHeap;
        try {
            messageOnHeap = _allocateStringOnHeap(message);
            intOnHeap = _allocateArrayOnHeap(new Int32Array(intArr));
            floatOnHeap = _allocateArrayOnHeap(new Float32Array(floatArr));
            bytesOnHeap = _allocateArrayOnHeap(new Uint8Array(byteArray));
            
            _SendMessage(messageOnHeap, intOnHeap.byteOffset, intArr.length, floatOnHeap.byteOffset, floatArr.length, bytesOnHeap.byteOffset, byteArray.length);
        }
        finally {
            _freeStringFromHeap(messageOnHeap);
            _freeArrayFromHeap(intOnHeap);
            _freeArrayFromHeap(floatOnHeap);
            _freeArrayFromHeap(bytesOnHeap);
        }
    };

    global["SendMessage"] = _sendMessage;
    module["SendMessage"] = _sendMessage;
})(this, Module);












/** @type {function(*, string=)} */
function assert(condition, text) {
  if (!condition) throw text;
}

function abort(what) {
  throw what;
}

var tempRet0 = 0;
var setTempRet0 = function(value) {
  tempRet0 = value;
}
var getTempRet0 = function() {
  return tempRet0;
}

function alignUp(x, multiple) {
  if (x % multiple > 0) {
    x += multiple - (x % multiple);
  }
  return x;
}




// Given a pointer 'ptr' to a null-terminated UTF8-encoded string in the given array that contains uint8 values, returns
// a copy of that string as a Javascript String object.

var UTF8Decoder = typeof TextDecoder !== 'undefined' ? new TextDecoder('utf8') : undefined;

/**
 * @param {number} idx
 * @param {number=} maxBytesToRead
 * @return {string}
 */
function UTF8ArrayToString(u8Array, idx, maxBytesToRead) {
  var endIdx = idx + maxBytesToRead;
  var endPtr = idx;
  // TextDecoder needs to know the byte length in advance, it doesn't stop on null terminator by itself.
  // Also, use the length info to avoid running tiny strings through TextDecoder, since .subarray() allocates garbage.
  // (As a tiny code save trick, compare endPtr against endIdx using a negation, so that undefined means Infinity)
  while (u8Array[endPtr] && !(endPtr >= endIdx)) ++endPtr;

  if (endPtr - idx > 16 && u8Array.subarray && UTF8Decoder) {
    return UTF8Decoder.decode(u8Array.subarray(idx, endPtr));
  } else {
    var str = '';
    // If building with TextDecoder, we have already computed the string length above, so test loop end condition against that
    while (idx < endPtr) {
      // For UTF8 byte structure, see:
      // http://en.wikipedia.org/wiki/UTF-8#Description
      // https://www.ietf.org/rfc/rfc2279.txt
      // https://tools.ietf.org/html/rfc3629
      var u0 = u8Array[idx++];
      if (!(u0 & 0x80)) { str += String.fromCharCode(u0); continue; }
      var u1 = u8Array[idx++] & 63;
      if ((u0 & 0xE0) == 0xC0) { str += String.fromCharCode(((u0 & 31) << 6) | u1); continue; }
      var u2 = u8Array[idx++] & 63;
      if ((u0 & 0xF0) == 0xE0) {
        u0 = ((u0 & 15) << 12) | (u1 << 6) | u2;
      } else {
        if ((u0 & 0xF8) != 0xF0) warnOnce('Invalid UTF-8 leading byte 0x' + u0.toString(16) + ' encountered when deserializing a UTF-8 string on the asm.js/wasm heap to a JS string!');
        u0 = ((u0 & 7) << 18) | (u1 << 12) | (u2 << 6) | (u8Array[idx++] & 63);
      }

      if (u0 < 0x10000) {
        str += String.fromCharCode(u0);
      } else {
        var ch = u0 - 0x10000;
        str += String.fromCharCode(0xD800 | (ch >> 10), 0xDC00 | (ch & 0x3FF));
      }
    }
  }
  return str;
}

// Given a pointer 'ptr' to a null-terminated UTF8-encoded string in the emscripten HEAP, returns a
// copy of that string as a Javascript String object.
// maxBytesToRead: an optional length that specifies the maximum number of bytes to read. You can omit
//                 this parameter to scan the string until the first \0 byte. If maxBytesToRead is
//                 passed, and the string at [ptr, ptr+maxBytesToReadr[ contains a null byte in the
//                 middle, then the string will cut short at that byte index (i.e. maxBytesToRead will
//                 not produce a string of exact length [ptr, ptr+maxBytesToRead[)
//                 N.B. mixing frequent uses of UTF8ToString() with and without maxBytesToRead may
//                 throw JS JIT optimizations off, so it is worth to consider consistently using one
//                 style or the other.
/**
 * @param {number} ptr
 * @param {number=} maxBytesToRead
 * @return {string}
 */
function UTF8ToString(ptr, maxBytesToRead) {
  return ptr ? UTF8ArrayToString(HEAPU8, ptr, maxBytesToRead) : '';
}

// Copies the given Javascript String object 'str' to the given byte array at address 'outIdx',
// encoded in UTF8 form and null-terminated. The copy will require at most str.length*4+1 bytes of space in the HEAP.
// Use the function lengthBytesUTF8 to compute the exact number of bytes (excluding null terminator) that this function will write.
// Parameters:
//   str: the Javascript string to copy.
//   outU8Array: the array to copy to. Each index in this array is assumed to be one 8-byte element.
//   outIdx: The starting offset in the array to begin the copying.
//   maxBytesToWrite: The maximum number of bytes this function can write to the array.
//                    This count should include the null terminator,
//                    i.e. if maxBytesToWrite=1, only the null terminator will be written and nothing else.
//                    maxBytesToWrite=0 does not write any bytes to the output, not even the null terminator.
// Returns the number of bytes written, EXCLUDING the null terminator.

function stringToUTF8Array(str, outU8Array, outIdx, maxBytesToWrite) {
  if (!(maxBytesToWrite > 0)) // Parameter maxBytesToWrite is not optional. Negative values, 0, null, undefined and false each don't write out any bytes.
    return 0;

  var startIdx = outIdx;
  var endIdx = outIdx + maxBytesToWrite - 1; // -1 for string null terminator.
  for (var i = 0; i < str.length; ++i) {
    // Gotcha: charCodeAt returns a 16-bit word that is a UTF-16 encoded code unit, not a Unicode code point of the character! So decode UTF16->UTF32->UTF8.
    // See http://unicode.org/faq/utf_bom.html#utf16-3
    // For UTF8 byte structure, see http://en.wikipedia.org/wiki/UTF-8#Description and https://www.ietf.org/rfc/rfc2279.txt and https://tools.ietf.org/html/rfc3629
    var u = str.charCodeAt(i); // possibly a lead surrogate
    if (u >= 0xD800 && u <= 0xDFFF) {
      var u1 = str.charCodeAt(++i);
      u = 0x10000 + ((u & 0x3FF) << 10) | (u1 & 0x3FF);
    }
    if (u <= 0x7F) {
      if (outIdx >= endIdx) break;
      outU8Array[outIdx++] = u;
    } else if (u <= 0x7FF) {
      if (outIdx + 1 >= endIdx) break;
      outU8Array[outIdx++] = 0xC0 | (u >> 6);
      outU8Array[outIdx++] = 0x80 | (u & 63);
    } else if (u <= 0xFFFF) {
      if (outIdx + 2 >= endIdx) break;
      outU8Array[outIdx++] = 0xE0 | (u >> 12);
      outU8Array[outIdx++] = 0x80 | ((u >> 6) & 63);
      outU8Array[outIdx++] = 0x80 | (u & 63);
    } else {
      if (outIdx + 3 >= endIdx) break;
      if (u >= 0x200000) warnOnce('Invalid Unicode code point 0x' + u.toString(16) + ' encountered when serializing a JS string to an UTF-8 string on the asm.js/wasm heap! (Valid unicode code points should be in range 0-0x1FFFFF).');
      outU8Array[outIdx++] = 0xF0 | (u >> 18);
      outU8Array[outIdx++] = 0x80 | ((u >> 12) & 63);
      outU8Array[outIdx++] = 0x80 | ((u >> 6) & 63);
      outU8Array[outIdx++] = 0x80 | (u & 63);
    }
  }
  // Null-terminate the pointer to the buffer.
  outU8Array[outIdx] = 0;
  return outIdx - startIdx;
}

// Copies the given Javascript String object 'str' to the emscripten HEAP at address 'outPtr',
// null-terminated and encoded in UTF8 form. The copy will require at most str.length*4+1 bytes of space in the HEAP.
// Use the function lengthBytesUTF8 to compute the exact number of bytes (excluding null terminator) that this function will write.
// Returns the number of bytes written, EXCLUDING the null terminator.

function stringToUTF8(str, outPtr, maxBytesToWrite) {
  assert(typeof maxBytesToWrite == 'number', 'stringToUTF8(str, outPtr, maxBytesToWrite) is missing the third parameter that specifies the length of the output buffer!');
  return stringToUTF8Array(str, HEAPU8,outPtr, maxBytesToWrite);
}

// Returns the number of bytes the given Javascript string takes if encoded as a UTF8 byte array, EXCLUDING the null terminator byte.
function lengthBytesUTF8(str) {
  var len = 0;
  for (var i = 0; i < str.length; ++i) {
    // Gotcha: charCodeAt returns a 16-bit word that is a UTF-16 encoded code unit, not a Unicode code point of the character! So decode UTF16->UTF32->UTF8.
    // See http://unicode.org/faq/utf_bom.html#utf16-3
    var u = str.charCodeAt(i); // possibly a lead surrogate
    if (u >= 0xD800 && u <= 0xDFFF) u = 0x10000 + ((u & 0x3FF) << 10) | (str.charCodeAt(++i) & 0x3FF);
    if (u <= 0x7F) ++len;
    else if (u <= 0x7FF) len += 2;
    else if (u <= 0xFFFF) len += 3;
    else len += 4;
  }
  return len;
}








var GLOBAL_BASE = 1024,
    TOTAL_STACK = 5242880,
    TOTAL_MEMORY = 134217728,
    STATIC_BASE = 1024,
    STACK_BASE = 807488,
    STACKTOP = STACK_BASE,
    STACK_MAX = 6050368
    , DYNAMICTOP_PTR = 807216
    ;


var wasmMaximumMemory = TOTAL_MEMORY;

var wasmMemory = new WebAssembly.Memory({
  'initial': TOTAL_MEMORY >> 16
  , 'maximum': wasmMaximumMemory >> 16
  });

var buffer = wasmMemory.buffer;




var WASM_PAGE_SIZE = 65536;
assert(STACK_BASE % 16 === 0, 'stack must start aligned to 16 bytes, STACK_BASE==' + STACK_BASE);
assert(TOTAL_MEMORY >= TOTAL_STACK, 'TOTAL_MEMORY should be larger than TOTAL_STACK, was ' + TOTAL_MEMORY + '! (TOTAL_STACK=' + TOTAL_STACK + ')');
assert((6050368) % 16 === 0, 'heap must start aligned to 16 bytes, DYNAMIC_BASE==' + 6050368);
assert(TOTAL_MEMORY % WASM_PAGE_SIZE === 0);
assert(buffer.byteLength === TOTAL_MEMORY);

var HEAP8 = new Int8Array(buffer);
var HEAP16 = new Int16Array(buffer);
var HEAP32 = new Int32Array(buffer);
var HEAPU8 = new Uint8Array(buffer);
var HEAPU16 = new Uint16Array(buffer);
var HEAPU32 = new Uint32Array(buffer);
var HEAPF32 = new Float32Array(buffer);
var HEAPF64 = new Float64Array(buffer);



  HEAP32[DYNAMICTOP_PTR>>2] = 6050368;



// Initializes the stack cookie. Called at the startup of main and at the startup of each thread in pthreads mode.
function writeStackCookie() {
  assert((STACK_MAX & 3) == 0);
  HEAPU32[(STACK_MAX >> 2)-1] = 0x02135467;
  HEAPU32[(STACK_MAX >> 2)-2] = 0x89BACDFE;
}

function checkStackCookie() {
  if (HEAPU32[(STACK_MAX >> 2)-1] != 0x02135467 || HEAPU32[(STACK_MAX >> 2)-2] != 0x89BACDFE) {
    abort('Stack overflow! Stack cookie has been overwritten, expected hex dwords 0x89BACDFE and 0x02135467, but received 0x' + HEAPU32[(STACK_MAX >> 2)-2].toString(16) + ' ' + HEAPU32[(STACK_MAX >> 2)-1].toString(16));
  }
  // Also test the global address 0 for integrity.
  if (HEAP32[0] !== 0x63736d65 /* 'emsc' */) throw 'Runtime error: The application has corrupted its heap memory area (address zero)!';
}



  HEAP32[0] = 0x63736d65; /* 'emsc' */




// Endianness check (note: assumes compiler arch was little-endian)
HEAP16[1] = 0x6373;
if (HEAPU8[2] !== 0x73 || HEAPU8[3] !== 0x63) throw 'Runtime error: expected the system to be little-endian!';

function abortFnPtrError(ptr, sig) {
	var possibleSig = '';
	for(var x in debug_tables) {
		var tbl = debug_tables[x];
		if (tbl[ptr]) {
			possibleSig += 'as sig "' + x + '" pointing to function ' + tbl[ptr] + ', ';
		}
	}
	abort("Invalid function pointer " + ptr + " called with signature '" + sig + "'. Perhaps this is an invalid value (e.g. caused by calling a virtual method on a NULL pointer)? Or calling a function with an incorrect type, which will fail? (it is worth building your source files with -Werror (warnings are errors), as warnings can indicate undefined behavior which can cause this). This pointer might make sense in another type signature: " + possibleSig);
}

function wrapAssertRuntimeReady(func) {
  var realFunc = asm[func];
  asm[func] = function() {
    assert(runtimeInitialized, 'you need to wait for the runtime to be ready (e.g. wait for main() to be called)');
    assert(!runtimeExited, 'the runtime was exited (use NO_EXIT_RUNTIME to keep it alive after main() exits)');
    return realFunc.apply(null, arguments);
  }
}




var runtimeInitialized = false;

// This is always false in minimal_runtime - the runtime does not have a concept of exiting (keeping this variable here for now since it is referenced from generated code)
var runtimeExited = false;

function unSign(value, bits, ignore) {
  if (value >= 0) {
    return value;
  }
  return bits <= 32 ? 2*Math.abs(1 << (bits-1)) + value // Need some trickery, since if bits == 32, we are right at the limit of the bits JS uses in bitshifts
                    : Math.pow(2, bits)         + value;
}
function reSign(value, bits, ignore) {
  if (value <= 0) {
    return value;
  }
  var half = bits <= 32 ? Math.abs(1 << (bits-1)) // abs is needed if bits == 32
                        : Math.pow(2, bits-1);
  if (value >= half && (bits <= 32 || value > half)) { // for huge values, we can hit the precision limit and always get true here. so don't do that
                                                       // but, in general there is no perfect solution here. With 64-bit ints, we get rounding and errors
                                                       // TODO: In i64 mode 1, resign the two parts separately and safely
    value = -2*half + value; // Cannot bitshift half, as it may be at the limit of the bits JS uses in bitshifts
  }
  return value;
}


assert(Math.imul, 'This browser does not support Math.imul(), build with LEGACY_VM_SUPPORT or POLYFILL_OLD_MATH_FUNCTIONS to add in a polyfill');
assert(Math.fround, 'This browser does not support Math.fround(), build with LEGACY_VM_SUPPORT or POLYFILL_OLD_MATH_FUNCTIONS to add in a polyfill');
assert(Math.clz32, 'This browser does not support Math.clz32(), build with LEGACY_VM_SUPPORT or POLYFILL_OLD_MATH_FUNCTIONS to add in a polyfill');
assert(Math.trunc, 'This browser does not support Math.trunc(), build with LEGACY_VM_SUPPORT or POLYFILL_OLD_MATH_FUNCTIONS to add in a polyfill');

var Math_abs = Math.abs;
var Math_cos = Math.cos;
var Math_sin = Math.sin;
var Math_tan = Math.tan;
var Math_acos = Math.acos;
var Math_asin = Math.asin;
var Math_atan = Math.atan;
var Math_atan2 = Math.atan2;
var Math_exp = Math.exp;
var Math_log = Math.log;
var Math_sqrt = Math.sqrt;
var Math_ceil = Math.ceil;
var Math_floor = Math.floor;
var Math_pow = Math.pow;
var Math_imul = Math.imul;
var Math_fround = Math.fround;
var Math_round = Math.round;
var Math_min = Math.min;
var Math_max = Math.max;
var Math_clz32 = Math.clz32;
var Math_trunc = Math.trunc;



var memoryInitializer = null;


// Copyright 2015 The Emscripten Authors.  All rights reserved.
// Emscripten is available under two separate licenses, the MIT license and the
// University of Illinois/NCSA Open Source License.  Both these licenses can be
// found in the LICENSE file.




// === Body ===

var ASM_CONSTS = [function() { debugger; }];

function _emscripten_asm_const_i(code) {
  return ASM_CONSTS[code]();
}




// STATICTOP = STATIC_BASE + 806464;









/* no memory initializer */
var tempDoublePtr = 807472
assert(tempDoublePtr % 8 == 0);

function copyTempFloat(ptr) { // functions, because inlining this code increases code size too much
  HEAP8[tempDoublePtr] = HEAP8[ptr];
  HEAP8[tempDoublePtr+1] = HEAP8[ptr+1];
  HEAP8[tempDoublePtr+2] = HEAP8[ptr+2];
  HEAP8[tempDoublePtr+3] = HEAP8[ptr+3];
}

function copyTempDouble(ptr) {
  HEAP8[tempDoublePtr] = HEAP8[ptr];
  HEAP8[tempDoublePtr+1] = HEAP8[ptr+1];
  HEAP8[tempDoublePtr+2] = HEAP8[ptr+2];
  HEAP8[tempDoublePtr+3] = HEAP8[ptr+3];
  HEAP8[tempDoublePtr+4] = HEAP8[ptr+4];
  HEAP8[tempDoublePtr+5] = HEAP8[ptr+5];
  HEAP8[tempDoublePtr+6] = HEAP8[ptr+6];
  HEAP8[tempDoublePtr+7] = HEAP8[ptr+7];
}

// {{PRE_LIBRARY}}


  function abortStackOverflow(allocSize) {
      abort('Stack overflow! Attempted to allocate ' + allocSize + ' bytes on the stack, but stack has only ' + (STACK_MAX - stackSave() + allocSize) + ' bytes available!');
    }

  function warnOnce(text) {
      if (!warnOnce.shown) warnOnce.shown = {};
      if (!warnOnce.shown[text]) {
        warnOnce.shown[text] = 1;
        err(text);
      }
    }

  
  var ___exception_infos={};
  
  var ___exception_caught= [];
  
  function ___exception_addRef(ptr) {
      if (!ptr) return;
      var info = ___exception_infos[ptr];
      info.refcount++;
    }
  
  function ___exception_deAdjust(adjusted) {
      if (!adjusted || ___exception_infos[adjusted]) return adjusted;
      for (var key in ___exception_infos) {
        var ptr = +key; // the iteration key is a string, and if we throw this, it must be an integer as that is what we look for
        var adj = ___exception_infos[ptr].adjusted;
        var len = adj.length;
        for (var i = 0; i < len; i++) {
          if (adj[i] === adjusted) {
            return ptr;
          }
        }
      }
      return adjusted;
    }function ___cxa_begin_catch(ptr) {
      var info = ___exception_infos[ptr];
      if (info && !info.caught) {
        info.caught = true;
        __ZSt18uncaught_exceptionv.uncaught_exception--;
      }
      if (info) info.rethrown = false;
      ___exception_caught.push(ptr);
      ___exception_addRef(___exception_deAdjust(ptr));
      return ptr;
    }

  function ___gxx_personality_v0() {
    }

  function ___lock() {}

  
  var SYSCALLS={buffers:[null,[],[]],printChar:function(stream, curr) {
        var buffer = SYSCALLS.buffers[stream];
        assert(buffer);
        if (curr === 0 || curr === 10) {
          (stream === 1 ? out : err)(UTF8ArrayToString(buffer, 0));
          buffer.length = 0;
        } else {
          buffer.push(curr);
        }
      },varargs:0,get:function(varargs) {
        SYSCALLS.varargs += 4;
        var ret = HEAP32[(((SYSCALLS.varargs)-(4))>>2)];
        return ret;
      },getStr:function() {
        var ret = UTF8ToString(SYSCALLS.get());
        return ret;
      },get64:function() {
        var low = SYSCALLS.get(), high = SYSCALLS.get();
        if (low >= 0) assert(high === 0);
        else assert(high === -1);
        return low;
      },getZero:function() {
        assert(SYSCALLS.get() === 0);
      }};function ___syscall140(which, varargs) {SYSCALLS.varargs = varargs;
  try {
   // llseek
      var stream = SYSCALLS.getStreamFromFD(), offset_high = SYSCALLS.get(), offset_low = SYSCALLS.get(), result = SYSCALLS.get(), whence = SYSCALLS.get();
      // NOTE: offset_high is unused - Emscripten's off_t is 32-bit
      var offset = offset_low;
      FS.llseek(stream, offset, whence);
      HEAP32[((result)>>2)]=stream.position;
      if (stream.getdents && offset === 0 && whence === 0) stream.getdents = null; // reset readdir state
      return 0;
    } catch (e) {
    if (typeof FS === 'undefined' || !(e instanceof FS.ErrnoError)) abort(e);
    return -e.errno;
  }
  }

  function ___syscall145(which, varargs) {SYSCALLS.varargs = varargs;
  try {
   // readv
      var stream = SYSCALLS.getStreamFromFD(), iov = SYSCALLS.get(), iovcnt = SYSCALLS.get();
      return SYSCALLS.doReadv(stream, iov, iovcnt);
    } catch (e) {
    if (typeof FS === 'undefined' || !(e instanceof FS.ErrnoError)) abort(e);
    return -e.errno;
  }
  }

  
  function flush_NO_FILESYSTEM() {
      // flush anything remaining in the buffers during shutdown
      var fflush = Module["_fflush"];
      if (fflush) fflush(0);
      var buffers = SYSCALLS.buffers;
      if (buffers[1].length) SYSCALLS.printChar(1, 10);
      if (buffers[2].length) SYSCALLS.printChar(2, 10);
    }function ___syscall146(which, varargs) {SYSCALLS.varargs = varargs;
  try {
   // writev
      // hack to support printf in FILESYSTEM=0
      var stream = SYSCALLS.get(), iov = SYSCALLS.get(), iovcnt = SYSCALLS.get();
      var ret = 0;
      for (var i = 0; i < iovcnt; i++) {
        var ptr = HEAP32[(((iov)+(i*8))>>2)];
        var len = HEAP32[(((iov)+(i*8 + 4))>>2)];
        for (var j = 0; j < len; j++) {
          SYSCALLS.printChar(stream, HEAPU8[ptr+j]);
        }
        ret += len;
      }
      return ret;
    } catch (e) {
    if (typeof FS === 'undefined' || !(e instanceof FS.ErrnoError)) abort(e);
    return -e.errno;
  }
  }

  
  function ___setErrNo(value) {
      return 0;
    }function ___syscall221(which, varargs) {SYSCALLS.varargs = varargs;
  try {
   // fcntl64
      return 0;
    } catch (e) {
    if (typeof FS === 'undefined' || !(e instanceof FS.ErrnoError)) abort(e);
    return -e.errno;
  }
  }

  function ___syscall4(which, varargs) {SYSCALLS.varargs = varargs;
  try {
   // write
      // hack to support printf in FILESYSTEM=0
      var stream = SYSCALLS.get(), buf = SYSCALLS.get(), count = SYSCALLS.get();
      for (var i = 0; i < count; i++) {
        SYSCALLS.printChar(stream, HEAPU8[buf+i]);
      }
      return count;
    } catch (e) {
    if (typeof FS === 'undefined' || !(e instanceof FS.ErrnoError)) abort(e);
    return -e.errno;
  }
  }

  function ___syscall5(which, varargs) {SYSCALLS.varargs = varargs;
  try {
   // open
      var pathname = SYSCALLS.getStr(), flags = SYSCALLS.get(), mode = SYSCALLS.get() // optional TODO
      var stream = FS.open(pathname, flags, mode);
      return stream.fd;
    } catch (e) {
    if (typeof FS === 'undefined' || !(e instanceof FS.ErrnoError)) abort(e);
    return -e.errno;
  }
  }

  function ___syscall54(which, varargs) {SYSCALLS.varargs = varargs;
  try {
   // ioctl
      return 0;
    } catch (e) {
    if (typeof FS === 'undefined' || !(e instanceof FS.ErrnoError)) abort(e);
    return -e.errno;
  }
  }

  function ___syscall6(which, varargs) {SYSCALLS.varargs = varargs;
  try {
   // close
      var stream = SYSCALLS.getStreamFromFD();
      FS.close(stream);
      return 0;
    } catch (e) {
    if (typeof FS === 'undefined' || !(e instanceof FS.ErrnoError)) abort(e);
    return -e.errno;
  }
  }

  function ___unlock() {}

  function _abort() {
      // In MINIMAL_RUNTIME the module object does not exist, so its behavior to abort is to throw directly.
      throw 'abort';
    }

  function _clock() {
      if (_clock.start === undefined) _clock.start = Date.now();
      return ((Date.now() - _clock.start) * (1000000 / 1000))|0;
    }

  
  function _emscripten_get_now() { abort() }
  
  function _emscripten_get_now_is_monotonic() {
      // return whether emscripten_get_now is guaranteed monotonic; the Date.now
      // implementation is not :(
      return (0
        || (typeof performance === 'object' && performance && typeof performance['now'] === 'function')
        );
    }function _clock_gettime(clk_id, tp) {
      // int clock_gettime(clockid_t clk_id, struct timespec *tp);
      var now;
      if (clk_id === 0) {
        now = Date.now();
      } else if (clk_id === 1 && _emscripten_get_now_is_monotonic()) {
        now = _emscripten_get_now();
      } else {
        ___setErrNo(22);
        return -1;
      }
      HEAP32[((tp)>>2)]=(now/1000)|0; // seconds
      HEAP32[(((tp)+(4))>>2)]=((now % 1000)*1000*1000)|0; // nanoseconds
      return 0;
    }

  var _emscripten_asm_const_int=true;


  
  var GL={counter:1,lastError:0,buffers:[],mappedBuffers:{},programs:[],framebuffers:[],renderbuffers:[],textures:[],uniforms:[],shaders:[],vaos:[],contexts:{},currentContext:null,offscreenCanvases:{},timerQueriesEXT:[],queries:[],samplers:[],transformFeedbacks:[],syncs:[],programInfos:{},stringCache:{},stringiCache:{},unpackAlignment:4,init:function() {
        GL.miniTempBuffer = new Float32Array(GL.MINI_TEMP_BUFFER_SIZE);
        for (var i = 0; i < GL.MINI_TEMP_BUFFER_SIZE; i++) {
          GL.miniTempBufferViews[i] = GL.miniTempBuffer.subarray(0, i+1);
        }
      },recordError:function recordError(errorCode) {
        if (!GL.lastError) {
          GL.lastError = errorCode;
        }
      },getNewId:function(table) {
        var ret = GL.counter++;
        for (var i = table.length; i < ret; i++) {
          table[i] = null;
        }
        return ret;
      },MINI_TEMP_BUFFER_SIZE:256,miniTempBuffer:null,miniTempBufferViews:[0],getSource:function(shader, count, string, length) {
        var source = '';
        for (var i = 0; i < count; ++i) {
          var len = length ? HEAP32[(((length)+(i*4))>>2)] : -1;
          source += UTF8ToString(HEAP32[(((string)+(i*4))>>2)], len < 0 ? undefined : len);
        }
        return source;
      },createContext:function(canvas, webGLContextAttributes) {
  
  
  
  
        var ctx = 
          (webGLContextAttributes.majorVersion > 1) ? canvas.getContext("webgl2", webGLContextAttributes) :
          (canvas.getContext("webgl", webGLContextAttributes) || canvas.getContext("experimental-webgl", webGLContextAttributes));
  
  
        return ctx && GL.registerContext(ctx, webGLContextAttributes);
      },registerContext:function(ctx, webGLContextAttributes) {
        var handle = _malloc(8); // Make space on the heap to store GL context attributes that need to be accessible as shared between threads.
        var context = {
          handle: handle,
          attributes: webGLContextAttributes,
          version: webGLContextAttributes.majorVersion,
          GLctx: ctx
        };
  
        // BUG: Workaround Chrome WebGL 2 issue: the first shipped versions of WebGL 2 in Chrome did not actually implement the new WebGL 2 functions.
        //      Those are supported only in Chrome 58 and newer.
        function getChromeVersion() {
          var raw = navigator.userAgent.match(/Chrom(e|ium)\/([0-9]+)\./);
          return raw ? parseInt(raw[2], 10) : false;
        }
        context.supportsWebGL2EntryPoints = (context.version >= 2) && (getChromeVersion() === false || getChromeVersion() >= 58);
  
  
        // Store the created context object so that we can access the context given a canvas without having to pass the parameters again.
        if (ctx.canvas) ctx.canvas.GLctxObject = context;
        GL.contexts[handle] = context;
        if (typeof webGLContextAttributes.enableExtensionsByDefault === 'undefined' || webGLContextAttributes.enableExtensionsByDefault) {
          GL.initExtensions(context);
        }
  
  
  
  
        return handle;
      },makeContextCurrent:function(contextHandle) {
  
        GL.currentContext = GL.contexts[contextHandle]; // Active Emscripten GL layer context object.
        Module.ctx = GLctx = GL.currentContext && GL.currentContext.GLctx; // Active WebGL context object.
        return !(contextHandle && !GLctx);
      },getContext:function(contextHandle) {
        return GL.contexts[contextHandle];
      },deleteContext:function(contextHandle) {
        if (GL.currentContext === GL.contexts[contextHandle]) GL.currentContext = null;
        if (typeof JSEvents === 'object') JSEvents.removeAllHandlersOnTarget(GL.contexts[contextHandle].GLctx.canvas); // Release all JS event handlers on the DOM element that the GL context is associated with since the context is now deleted.
        if (GL.contexts[contextHandle] && GL.contexts[contextHandle].GLctx.canvas) GL.contexts[contextHandle].GLctx.canvas.GLctxObject = undefined; // Make sure the canvas object no longer refers to the context object so there are no GC surprises.
        _free(GL.contexts[contextHandle]);
        GL.contexts[contextHandle] = null;
      },initExtensions:function(context) {
        // If this function is called without a specific context object, init the extensions of the currently active context.
        if (!context) context = GL.currentContext;
  
        if (context.initExtensionsDone) return;
        context.initExtensionsDone = true;
  
        var GLctx = context.GLctx;
  
        // Detect the presence of a few extensions manually, this GL interop layer itself will need to know if they exist.
  
        if (context.version < 2) {
          // Extension available from Firefox 26 and Google Chrome 30
          var instancedArraysExt = GLctx.getExtension('ANGLE_instanced_arrays');
          if (instancedArraysExt) {
            GLctx['vertexAttribDivisor'] = function(index, divisor) { instancedArraysExt['vertexAttribDivisorANGLE'](index, divisor); };
            GLctx['drawArraysInstanced'] = function(mode, first, count, primcount) { instancedArraysExt['drawArraysInstancedANGLE'](mode, first, count, primcount); };
            GLctx['drawElementsInstanced'] = function(mode, count, type, indices, primcount) { instancedArraysExt['drawElementsInstancedANGLE'](mode, count, type, indices, primcount); };
          }
  
          // Extension available from Firefox 25 and WebKit
          var vaoExt = GLctx.getExtension('OES_vertex_array_object');
          if (vaoExt) {
            GLctx['createVertexArray'] = function() { return vaoExt['createVertexArrayOES'](); };
            GLctx['deleteVertexArray'] = function(vao) { vaoExt['deleteVertexArrayOES'](vao); };
            GLctx['bindVertexArray'] = function(vao) { vaoExt['bindVertexArrayOES'](vao); };
            GLctx['isVertexArray'] = function(vao) { return vaoExt['isVertexArrayOES'](vao); };
          }
  
          var drawBuffersExt = GLctx.getExtension('WEBGL_draw_buffers');
          if (drawBuffersExt) {
            GLctx['drawBuffers'] = function(n, bufs) { drawBuffersExt['drawBuffersWEBGL'](n, bufs); };
          }
        }
  
        GLctx.disjointTimerQueryExt = GLctx.getExtension("EXT_disjoint_timer_query");
  
        // These are the 'safe' feature-enabling extensions that don't add any performance impact related to e.g. debugging, and
        // should be enabled by default so that client GLES2/GL code will not need to go through extra hoops to get its stuff working.
        // As new extensions are ratified at http://www.khronos.org/registry/webgl/extensions/ , feel free to add your new extensions
        // here, as long as they don't produce a performance impact for users that might not be using those extensions.
        // E.g. debugging-related extensions should probably be off by default.
        var automaticallyEnabledExtensions = [ // Khronos ratified WebGL extensions ordered by number (no debug extensions):
                                               "OES_texture_float", "OES_texture_half_float", "OES_standard_derivatives",
                                               "OES_vertex_array_object", "WEBGL_compressed_texture_s3tc", "WEBGL_depth_texture",
                                               "OES_element_index_uint", "EXT_texture_filter_anisotropic", "EXT_frag_depth",
                                               "WEBGL_draw_buffers", "ANGLE_instanced_arrays", "OES_texture_float_linear",
                                               "OES_texture_half_float_linear", "EXT_blend_minmax", "EXT_shader_texture_lod",
                                               // Community approved WebGL extensions ordered by number:
                                               "WEBGL_compressed_texture_pvrtc", "EXT_color_buffer_half_float", "WEBGL_color_buffer_float",
                                               "EXT_sRGB", "WEBGL_compressed_texture_etc1", "EXT_disjoint_timer_query",
                                               "WEBGL_compressed_texture_etc", "WEBGL_compressed_texture_astc", "EXT_color_buffer_float",
                                               "WEBGL_compressed_texture_s3tc_srgb", "EXT_disjoint_timer_query_webgl2"];
  
        function shouldEnableAutomatically(extension) {
          var ret = false;
          automaticallyEnabledExtensions.forEach(function(include) {
            if (extension.indexOf(include) != -1) {
              ret = true;
            }
          });
          return ret;
        }
  
        var exts = GLctx.getSupportedExtensions();
        if (exts && exts.length > 0) {
          GLctx.getSupportedExtensions().forEach(function(ext) {
            if (automaticallyEnabledExtensions.indexOf(ext) != -1) {
              GLctx.getExtension(ext); // Calling .getExtension enables that extension permanently, no need to store the return value to be enabled.
            }
          });
        }
      },populateUniformTable:function(program) {
        var p = GL.programs[program];
        var ptable = GL.programInfos[program] = {
          uniforms: {},
          maxUniformLength: 0, // This is eagerly computed below, since we already enumerate all uniforms anyway.
          maxAttributeLength: -1, // This is lazily computed and cached, computed when/if first asked, "-1" meaning not computed yet.
          maxUniformBlockNameLength: -1 // Lazily computed as well
        };
  
        var utable = ptable.uniforms;
        // A program's uniform table maps the string name of an uniform to an integer location of that uniform.
        // The global GL.uniforms map maps integer locations to WebGLUniformLocations.
        var numUniforms = GLctx.getProgramParameter(p, 0x8B86/*GL_ACTIVE_UNIFORMS*/);
        for (var i = 0; i < numUniforms; ++i) {
          var u = GLctx.getActiveUniform(p, i);
  
          var name = u.name;
          ptable.maxUniformLength = Math.max(ptable.maxUniformLength, name.length+1);
  
          // If we are dealing with an array, e.g. vec4 foo[3], strip off the array index part to canonicalize that "foo", "foo[]",
          // and "foo[0]" will mean the same. Loop below will populate foo[1] and foo[2].
          if (name.slice(-1) == ']') {
            name = name.slice(0, name.lastIndexOf('['));
          }
  
          // Optimize memory usage slightly: If we have an array of uniforms, e.g. 'vec3 colors[3];', then
          // only store the string 'colors' in utable, and 'colors[0]', 'colors[1]' and 'colors[2]' will be parsed as 'colors'+i.
          // Note that for the GL.uniforms table, we still need to fetch the all WebGLUniformLocations for all the indices.
          var loc = GLctx.getUniformLocation(p, name);
          if (loc) {
            var id = GL.getNewId(GL.uniforms);
            utable[name] = [u.size, id];
            GL.uniforms[id] = loc;
  
            for (var j = 1; j < u.size; ++j) {
              var n = name + '['+j+']';
              loc = GLctx.getUniformLocation(p, n);
              id = GL.getNewId(GL.uniforms);
  
              GL.uniforms[id] = loc;
            }
          }
        }
      }};function _emscripten_glActiveTexture(x0) { GLctx['activeTexture'](x0) }

  function _emscripten_glAttachShader(program, shader) {
      GLctx.attachShader(GL.programs[program],
                              GL.shaders[shader]);
    }

  function _emscripten_glBeginQuery(target, id) {
      GLctx['beginQuery'](target, GL.queries[id]);
    }

  function _emscripten_glBeginQueryEXT(target, id) {
      GLctx.disjointTimerQueryExt['beginQueryEXT'](target, GL.timerQueriesEXT[id]);
    }

  function _emscripten_glBeginTransformFeedback(x0) { GLctx['beginTransformFeedback'](x0) }

  function _emscripten_glBindAttribLocation(program, index, name) {
      GLctx.bindAttribLocation(GL.programs[program], index, UTF8ToString(name));
    }

  function _emscripten_glBindBuffer(target, buffer) {
  
      if (target == 0x88EB /*GL_PIXEL_PACK_BUFFER*/) {
        // In WebGL 2 glReadPixels entry point, we need to use a different WebGL 2 API function call when a buffer is bound to
        // GL_PIXEL_PACK_BUFFER_BINDING point, so must keep track whether that binding point is non-null to know what is
        // the proper API function to call.
        GLctx.currentPixelPackBufferBinding = buffer;
      } else if (target == 0x88EC /*GL_PIXEL_UNPACK_BUFFER*/) {
        // In WebGL 2 gl(Compressed)Tex(Sub)Image[23]D entry points, we need to
        // use a different WebGL 2 API function call when a buffer is bound to
        // GL_PIXEL_UNPACK_BUFFER_BINDING point, so must keep track whether that
        // binding point is non-null to know what is the proper API function to
        // call.
        GLctx.currentPixelUnpackBufferBinding = buffer;
      }
      GLctx.bindBuffer(target, GL.buffers[buffer]);
    }

  function _emscripten_glBindBufferBase(target, index, buffer) {
      GLctx['bindBufferBase'](target, index, GL.buffers[buffer]);
    }

  function _emscripten_glBindBufferRange(target, index, buffer, offset, ptrsize) {
      GLctx['bindBufferRange'](target, index, GL.buffers[buffer], offset, ptrsize);
    }

  function _emscripten_glBindFramebuffer(target, framebuffer) {
  
      GLctx.bindFramebuffer(target, GL.framebuffers[framebuffer]);
  
    }

  function _emscripten_glBindRenderbuffer(target, renderbuffer) {
      GLctx.bindRenderbuffer(target, GL.renderbuffers[renderbuffer]);
    }

  function _emscripten_glBindSampler(unit, sampler) {
      GLctx['bindSampler'](unit, GL.samplers[sampler]);
    }

  function _emscripten_glBindTexture(target, texture) {
      GLctx.bindTexture(target, GL.textures[texture]);
    }

  function _emscripten_glBindTransformFeedback(target, id) {
      GLctx['bindTransformFeedback'](target, GL.transformFeedbacks[id]);
    }

  function _emscripten_glBindVertexArray(vao) {
      GLctx['bindVertexArray'](GL.vaos[vao]);
    }

  function _emscripten_glBindVertexArrayOES(vao) {
      GLctx['bindVertexArray'](GL.vaos[vao]);
    }

  function _emscripten_glBlendColor(x0, x1, x2, x3) { GLctx['blendColor'](x0, x1, x2, x3) }

  function _emscripten_glBlendEquation(x0) { GLctx['blendEquation'](x0) }

  function _emscripten_glBlendEquationSeparate(x0, x1) { GLctx['blendEquationSeparate'](x0, x1) }

  function _emscripten_glBlendFunc(x0, x1) { GLctx['blendFunc'](x0, x1) }

  function _emscripten_glBlendFuncSeparate(x0, x1, x2, x3) { GLctx['blendFuncSeparate'](x0, x1, x2, x3) }

  function _emscripten_glBlitFramebuffer(x0, x1, x2, x3, x4, x5, x6, x7, x8, x9) { GLctx['blitFramebuffer'](x0, x1, x2, x3, x4, x5, x6, x7, x8, x9) }

  function _emscripten_glBufferData(target, size, data, usage) {
      if (GL.currentContext.supportsWebGL2EntryPoints) { // WebGL 2 provides new garbage-free entry points to call to WebGL. Use those always when possible.
        if (data) {
          GLctx.bufferData(target, HEAPU8, usage, data, size);
        } else {
          GLctx.bufferData(target, size, usage);
        }
      } else {
        // N.b. here first form specifies a heap subarray, second form an integer size, so the ?: code here is polymorphic. It is advised to avoid
        // randomly mixing both uses in calling code, to avoid any potential JS engine JIT issues.
        GLctx.bufferData(target, data ? HEAPU8.subarray(data, data+size) : size, usage);
      }
    }

  function _emscripten_glBufferSubData(target, offset, size, data) {
      if (GL.currentContext.supportsWebGL2EntryPoints) { // WebGL 2 provides new garbage-free entry points to call to WebGL. Use those always when possible.
        GLctx.bufferSubData(target, offset, HEAPU8, data, size);
        return;
      }
      GLctx.bufferSubData(target, offset, HEAPU8.subarray(data, data+size));
    }

  function _emscripten_glCheckFramebufferStatus(x0) { return GLctx['checkFramebufferStatus'](x0) }

  function _emscripten_glClear(x0) { GLctx['clear'](x0) }

  function _emscripten_glClearBufferfi(x0, x1, x2, x3) { GLctx['clearBufferfi'](x0, x1, x2, x3) }

  function _emscripten_glClearBufferfv(buffer, drawbuffer, value) {
  
      GLctx['clearBufferfv'](buffer, drawbuffer, HEAPF32, value>>2);
    }

  function _emscripten_glClearBufferiv(buffer, drawbuffer, value) {
  
      GLctx['clearBufferiv'](buffer, drawbuffer, HEAP32, value>>2);
    }

  function _emscripten_glClearBufferuiv(buffer, drawbuffer, value) {
  
      GLctx['clearBufferuiv'](buffer, drawbuffer, HEAPU32, value>>2);
    }

  function _emscripten_glClearColor(x0, x1, x2, x3) { GLctx['clearColor'](x0, x1, x2, x3) }

  function _emscripten_glClearDepthf(x0) { GLctx['clearDepth'](x0) }

  function _emscripten_glClearStencil(x0) { GLctx['clearStencil'](x0) }

  function _emscripten_glClientWaitSync(sync, flags, timeoutLo, timeoutHi) {
      // WebGL2 vs GLES3 differences: in GLES3, the timeout parameter is a uint64, where 0xFFFFFFFFFFFFFFFFULL means GL_TIMEOUT_IGNORED.
      // In JS, there's no 64-bit value types, so instead timeout is taken to be signed, and GL_TIMEOUT_IGNORED is given value -1.
      // Inherently the value accepted in the timeout is lossy, and can't take in arbitrary u64 bit pattern (but most likely doesn't matter)
      // See https://www.khronos.org/registry/webgl/specs/latest/2.0/#5.15
      timeoutLo = timeoutLo >>> 0;
      timeoutHi = timeoutHi >>> 0;
      var timeout = (timeoutLo == 0xFFFFFFFF && timeoutHi == 0xFFFFFFFF) ? -1 : makeBigInt(timeoutLo, timeoutHi, true);
      return GLctx.clientWaitSync(GL.syncs[sync], flags, timeout);
    }

  function _emscripten_glColorMask(red, green, blue, alpha) {
      GLctx.colorMask(!!red, !!green, !!blue, !!alpha);
    }

  function _emscripten_glCompileShader(shader) {
      GLctx.compileShader(GL.shaders[shader]);
    }

  function _emscripten_glCompressedTexImage2D(target, level, internalFormat, width, height, border, imageSize, data) {
      if (GL.currentContext.supportsWebGL2EntryPoints) { // WebGL 2 provides new garbage-free entry points to call to WebGL. Use those always when possible.
        if (GLctx.currentPixelUnpackBufferBinding) {
          GLctx['compressedTexImage2D'](target, level, internalFormat, width, height, border, imageSize, data);
        } else {
          GLctx['compressedTexImage2D'](target, level, internalFormat, width, height, border, HEAPU8, data, imageSize);
        }
        return;
      }
      GLctx['compressedTexImage2D'](target, level, internalFormat, width, height, border, data ? HEAPU8.subarray((data),(data+imageSize)) : null);
    }

  function _emscripten_glCompressedTexImage3D(target, level, internalFormat, width, height, depth, border, imageSize, data) {
      if (GL.currentContext.supportsWebGL2EntryPoints) { // WebGL 2 provides new garbage-free entry points to call to WebGL. Use those always when possible.
        if (GLctx.currentPixelUnpackBufferBinding) {
          GLctx['compressedTexImage3D'](target, level, internalFormat, width, height, depth, border, imageSize, data);
        } else {
          GLctx['compressedTexImage3D'](target, level, internalFormat, width, height, depth, border, HEAPU8, data, imageSize);
        }
      } else {
        GLctx['compressedTexImage3D'](target, level, internalFormat, width, height, depth, border, data ? HEAPU8.subarray((data),(data+imageSize)) : null);
      }
    }

  function _emscripten_glCompressedTexSubImage2D(target, level, xoffset, yoffset, width, height, format, imageSize, data) {
      if (GL.currentContext.supportsWebGL2EntryPoints) { // WebGL 2 provides new garbage-free entry points to call to WebGL. Use those always when possible.
        if (GLctx.currentPixelUnpackBufferBinding) {
          GLctx['compressedTexSubImage2D'](target, level, xoffset, yoffset, width, height, format, imageSize, data);
        } else {
          GLctx['compressedTexSubImage2D'](target, level, xoffset, yoffset, width, height, format, HEAPU8, data, imageSize);
        }
        return;
      }
      GLctx['compressedTexSubImage2D'](target, level, xoffset, yoffset, width, height, format, data ? HEAPU8.subarray((data),(data+imageSize)) : null);
    }

  function _emscripten_glCompressedTexSubImage3D(target, level, xoffset, yoffset, zoffset, width, height, depth, format, imageSize, data) {
      if (GL.currentContext.supportsWebGL2EntryPoints) { // WebGL 2 provides new garbage-free entry points to call to WebGL. Use those always when possible.
        if (GLctx.currentPixelUnpackBufferBinding) {
          GLctx['compressedTexSubImage3D'](target, level, xoffset, yoffset, zoffset, width, height, depth, format, imageSize, data);
        } else {
          GLctx['compressedTexSubImage3D'](target, level, xoffset, yoffset, zoffset, width, height, depth, format, HEAPU8, data, imageSize);
        }
      } else {
        GLctx['compressedTexSubImage3D'](target, level, xoffset, yoffset, zoffset, width, height, depth, format, data ? HEAPU8.subarray((data),(data+imageSize)) : null);
      }
    }

  function _emscripten_glCopyBufferSubData(x0, x1, x2, x3, x4) { GLctx['copyBufferSubData'](x0, x1, x2, x3, x4) }

  function _emscripten_glCopyTexImage2D(x0, x1, x2, x3, x4, x5, x6, x7) { GLctx['copyTexImage2D'](x0, x1, x2, x3, x4, x5, x6, x7) }

  function _emscripten_glCopyTexSubImage2D(x0, x1, x2, x3, x4, x5, x6, x7) { GLctx['copyTexSubImage2D'](x0, x1, x2, x3, x4, x5, x6, x7) }

  function _emscripten_glCopyTexSubImage3D(x0, x1, x2, x3, x4, x5, x6, x7, x8) { GLctx['copyTexSubImage3D'](x0, x1, x2, x3, x4, x5, x6, x7, x8) }

  function _emscripten_glCreateProgram() {
      var id = GL.getNewId(GL.programs);
      var program = GLctx.createProgram();
      program.name = id;
      GL.programs[id] = program;
      return id;
    }

  function _emscripten_glCreateShader(shaderType) {
      var id = GL.getNewId(GL.shaders);
      GL.shaders[id] = GLctx.createShader(shaderType);
      return id;
    }

  function _emscripten_glCullFace(x0) { GLctx['cullFace'](x0) }

  function _emscripten_glDeleteBuffers(n, buffers) {
      for (var i = 0; i < n; i++) {
        var id = HEAP32[(((buffers)+(i*4))>>2)];
        var buffer = GL.buffers[id];
  
        // From spec: "glDeleteBuffers silently ignores 0's and names that do not
        // correspond to existing buffer objects."
        if (!buffer) continue;
  
        GLctx.deleteBuffer(buffer);
        buffer.name = 0;
        GL.buffers[id] = null;
  
        if (id == GL.currArrayBuffer) GL.currArrayBuffer = 0;
        if (id == GL.currElementArrayBuffer) GL.currElementArrayBuffer = 0;
        if (id == GLctx.currentPixelPackBufferBinding) GLctx.currentPixelPackBufferBinding = 0;
        if (id == GLctx.currentPixelUnpackBufferBinding) GLctx.currentPixelUnpackBufferBinding = 0;
      }
    }

  function _emscripten_glDeleteFramebuffers(n, framebuffers) {
      for (var i = 0; i < n; ++i) {
        var id = HEAP32[(((framebuffers)+(i*4))>>2)];
        var framebuffer = GL.framebuffers[id];
        if (!framebuffer) continue; // GL spec: "glDeleteFramebuffers silently ignores 0s and names that do not correspond to existing framebuffer objects".
        GLctx.deleteFramebuffer(framebuffer);
        framebuffer.name = 0;
        GL.framebuffers[id] = null;
      }
    }

  function _emscripten_glDeleteProgram(id) {
      if (!id) return;
      var program = GL.programs[id];
      if (!program) { // glDeleteProgram actually signals an error when deleting a nonexisting object, unlike some other GL delete functions.
        GL.recordError(0x0501 /* GL_INVALID_VALUE */);
        return;
      }
      GLctx.deleteProgram(program);
      program.name = 0;
      GL.programs[id] = null;
      GL.programInfos[id] = null;
    }

  function _emscripten_glDeleteQueries(n, ids) {
      for (var i = 0; i < n; i++) {
        var id = HEAP32[(((ids)+(i*4))>>2)];
        var query = GL.queries[id];
        if (!query) continue; // GL spec: "unused names in ids are ignored, as is the name zero."
        GLctx['deleteQuery'](query);
        GL.queries[id] = null;
      }
    }

  function _emscripten_glDeleteQueriesEXT(n, ids) {
      for (var i = 0; i < n; i++) {
        var id = HEAP32[(((ids)+(i*4))>>2)];
        var query = GL.timerQueriesEXT[id];
        if (!query) continue; // GL spec: "unused names in ids are ignored, as is the name zero."
        GLctx.disjointTimerQueryExt['deleteQueryEXT'](query);
        GL.timerQueriesEXT[id] = null;
      }
    }

  function _emscripten_glDeleteRenderbuffers(n, renderbuffers) {
      for (var i = 0; i < n; i++) {
        var id = HEAP32[(((renderbuffers)+(i*4))>>2)];
        var renderbuffer = GL.renderbuffers[id];
        if (!renderbuffer) continue; // GL spec: "glDeleteRenderbuffers silently ignores 0s and names that do not correspond to existing renderbuffer objects".
        GLctx.deleteRenderbuffer(renderbuffer);
        renderbuffer.name = 0;
        GL.renderbuffers[id] = null;
      }
    }

  function _emscripten_glDeleteSamplers(n, samplers) {
      for (var i = 0; i < n; i++) {
        var id = HEAP32[(((samplers)+(i*4))>>2)];
        var sampler = GL.samplers[id];
        if (!sampler) continue;
        GLctx['deleteSampler'](sampler);
        sampler.name = 0;
        GL.samplers[id] = null;
      }
    }

  function _emscripten_glDeleteShader(id) {
      if (!id) return;
      var shader = GL.shaders[id];
      if (!shader) { // glDeleteShader actually signals an error when deleting a nonexisting object, unlike some other GL delete functions.
        GL.recordError(0x0501 /* GL_INVALID_VALUE */);
        return;
      }
      GLctx.deleteShader(shader);
      GL.shaders[id] = null;
    }

  function _emscripten_glDeleteSync(id) {
      if (!id) return;
      var sync = GL.syncs[id];
      if (!sync) { // glDeleteSync signals an error when deleting a nonexisting object, unlike some other GL delete functions.
        GL.recordError(0x0501 /* GL_INVALID_VALUE */);
        return;
      }
      GLctx.deleteSync(sync);
      sync.name = 0;
      GL.syncs[id] = null;
    }

  function _emscripten_glDeleteTextures(n, textures) {
      for (var i = 0; i < n; i++) {
        var id = HEAP32[(((textures)+(i*4))>>2)];
        var texture = GL.textures[id];
        if (!texture) continue; // GL spec: "glDeleteTextures silently ignores 0s and names that do not correspond to existing textures".
        GLctx.deleteTexture(texture);
        texture.name = 0;
        GL.textures[id] = null;
      }
    }

  function _emscripten_glDeleteTransformFeedbacks(n, ids) {
      for (var i = 0; i < n; i++) {
        var id = HEAP32[(((ids)+(i*4))>>2)];
        var transformFeedback = GL.transformFeedbacks[id];
        if (!transformFeedback) continue; // GL spec: "unused names in ids are ignored, as is the name zero."
        GLctx['deleteTransformFeedback'](transformFeedback);
        transformFeedback.name = 0;
        GL.transformFeedbacks[id] = null;
      }
    }

  function _emscripten_glDeleteVertexArrays(n, vaos) {
      for (var i = 0; i < n; i++) {
        var id = HEAP32[(((vaos)+(i*4))>>2)];
        GLctx['deleteVertexArray'](GL.vaos[id]);
        GL.vaos[id] = null;
      }
    }

  function _emscripten_glDeleteVertexArraysOES(n, vaos) {
      for (var i = 0; i < n; i++) {
        var id = HEAP32[(((vaos)+(i*4))>>2)];
        GLctx['deleteVertexArray'](GL.vaos[id]);
        GL.vaos[id] = null;
      }
    }

  function _emscripten_glDepthFunc(x0) { GLctx['depthFunc'](x0) }

  function _emscripten_glDepthMask(flag) {
      GLctx.depthMask(!!flag);
    }

  function _emscripten_glDepthRangef(x0, x1) { GLctx['depthRange'](x0, x1) }

  function _emscripten_glDetachShader(program, shader) {
      GLctx.detachShader(GL.programs[program],
                              GL.shaders[shader]);
    }

  function _emscripten_glDisable(x0) { GLctx['disable'](x0) }

  function _emscripten_glDisableVertexAttribArray(index) {
      GLctx.disableVertexAttribArray(index);
    }

  function _emscripten_glDrawArrays(mode, first, count) {
  
      GLctx.drawArrays(mode, first, count);
  
    }

  function _emscripten_glDrawArraysInstanced(mode, first, count, primcount) {
      GLctx['drawArraysInstanced'](mode, first, count, primcount);
    }

  function _emscripten_glDrawArraysInstancedANGLE(mode, first, count, primcount) {
      GLctx['drawArraysInstanced'](mode, first, count, primcount);
    }

  function _emscripten_glDrawArraysInstancedARB(mode, first, count, primcount) {
      GLctx['drawArraysInstanced'](mode, first, count, primcount);
    }

  function _emscripten_glDrawArraysInstancedEXT(mode, first, count, primcount) {
      GLctx['drawArraysInstanced'](mode, first, count, primcount);
    }

  function _emscripten_glDrawArraysInstancedNV(mode, first, count, primcount) {
      GLctx['drawArraysInstanced'](mode, first, count, primcount);
    }

  
  var __tempFixedLengthArray=[];function _emscripten_glDrawBuffers(n, bufs) {
  
      var bufArray = __tempFixedLengthArray[n];
      for (var i = 0; i < n; i++) {
        bufArray[i] = HEAP32[(((bufs)+(i*4))>>2)];
      }
  
      GLctx['drawBuffers'](bufArray);
    }

  function _emscripten_glDrawBuffersEXT(n, bufs) {
  
      var bufArray = __tempFixedLengthArray[n];
      for (var i = 0; i < n; i++) {
        bufArray[i] = HEAP32[(((bufs)+(i*4))>>2)];
      }
  
      GLctx['drawBuffers'](bufArray);
    }

  function _emscripten_glDrawBuffersWEBGL(n, bufs) {
  
      var bufArray = __tempFixedLengthArray[n];
      for (var i = 0; i < n; i++) {
        bufArray[i] = HEAP32[(((bufs)+(i*4))>>2)];
      }
  
      GLctx['drawBuffers'](bufArray);
    }

  function _emscripten_glDrawElements(mode, count, type, indices) {
  
      GLctx.drawElements(mode, count, type, indices);
  
    }

  function _emscripten_glDrawElementsInstanced(mode, count, type, indices, primcount) {
      GLctx['drawElementsInstanced'](mode, count, type, indices, primcount);
    }

  function _emscripten_glDrawElementsInstancedANGLE(mode, count, type, indices, primcount) {
      GLctx['drawElementsInstanced'](mode, count, type, indices, primcount);
    }

  function _emscripten_glDrawElementsInstancedARB(mode, count, type, indices, primcount) {
      GLctx['drawElementsInstanced'](mode, count, type, indices, primcount);
    }

  function _emscripten_glDrawElementsInstancedEXT(mode, count, type, indices, primcount) {
      GLctx['drawElementsInstanced'](mode, count, type, indices, primcount);
    }

  function _emscripten_glDrawElementsInstancedNV(mode, count, type, indices, primcount) {
      GLctx['drawElementsInstanced'](mode, count, type, indices, primcount);
    }

  
  function _glDrawElements(mode, count, type, indices) {
  
      GLctx.drawElements(mode, count, type, indices);
  
    }function _emscripten_glDrawRangeElements(mode, start, end, count, type, indices) {
      // TODO: This should be a trivial pass-though function registered at the bottom of this page as
      // glFuncs[6][1] += ' drawRangeElements';
      // but due to https://bugzilla.mozilla.org/show_bug.cgi?id=1202427,
      // we work around by ignoring the range.
      _glDrawElements(mode, count, type, indices);
    }

  function _emscripten_glEnable(x0) { GLctx['enable'](x0) }

  function _emscripten_glEnableVertexAttribArray(index) {
      GLctx.enableVertexAttribArray(index);
    }

  function _emscripten_glEndQuery(x0) { GLctx['endQuery'](x0) }

  function _emscripten_glEndQueryEXT(target) {
      GLctx.disjointTimerQueryExt['endQueryEXT'](target);
    }

  function _emscripten_glEndTransformFeedback() { GLctx['endTransformFeedback']() }

  function _emscripten_glFenceSync(condition, flags) {
      var sync = GLctx.fenceSync(condition, flags);
      if (sync) {
        var id = GL.getNewId(GL.syncs);
        sync.name = id;
        GL.syncs[id] = sync;
        return id;
      } else {
        return 0; // Failed to create a sync object
      }
    }

  function _emscripten_glFinish() { GLctx['finish']() }

  function _emscripten_glFlush() { GLctx['flush']() }

  function _emscripten_glFlushMappedBufferRange(
  ) {
  err('missing function: emscripten_glFlushMappedBufferRange'); abort(-1);
  }

  function _emscripten_glFramebufferRenderbuffer(target, attachment, renderbuffertarget, renderbuffer) {
      GLctx.framebufferRenderbuffer(target, attachment, renderbuffertarget,
                                         GL.renderbuffers[renderbuffer]);
    }

  function _emscripten_glFramebufferTexture2D(target, attachment, textarget, texture, level) {
      GLctx.framebufferTexture2D(target, attachment, textarget,
                                      GL.textures[texture], level);
    }

  function _emscripten_glFramebufferTextureLayer(target, attachment, texture, level, layer) {
      GLctx.framebufferTextureLayer(target, attachment, GL.textures[texture], level, layer);
    }

  function _emscripten_glFrontFace(x0) { GLctx['frontFace'](x0) }

  
  function __glGenObject(n, buffers, createFunction, objectTable
      ) {
      for (var i = 0; i < n; i++) {
        var buffer = GLctx[createFunction]();
        var id = buffer && GL.getNewId(objectTable);
        if (buffer) {
          buffer.name = id;
          objectTable[id] = buffer;
        } else {
          GL.recordError(0x0502 /* GL_INVALID_OPERATION */);
        }
        HEAP32[(((buffers)+(i*4))>>2)]=id;
      }
    }function _emscripten_glGenBuffers(n, buffers) {
      __glGenObject(n, buffers, 'createBuffer', GL.buffers
        );
    }

  function _emscripten_glGenFramebuffers(n, ids) {
      __glGenObject(n, ids, 'createFramebuffer', GL.framebuffers
        );
    }

  function _emscripten_glGenQueries(n, ids) {
      __glGenObject(n, ids, 'createQuery', GL.queries
        );
    }

  function _emscripten_glGenQueriesEXT(n, ids) {
      for (var i = 0; i < n; i++) {
        var query = GLctx.disjointTimerQueryExt['createQueryEXT']();
        if (!query) {
          GL.recordError(0x0502 /* GL_INVALID_OPERATION */);
          while(i < n) HEAP32[(((ids)+(i++*4))>>2)]=0;
          return;
        }
        var id = GL.getNewId(GL.timerQueriesEXT);
        query.name = id;
        GL.timerQueriesEXT[id] = query;
        HEAP32[(((ids)+(i*4))>>2)]=id;
      }
    }

  function _emscripten_glGenRenderbuffers(n, renderbuffers) {
      __glGenObject(n, renderbuffers, 'createRenderbuffer', GL.renderbuffers
        );
    }

  function _emscripten_glGenSamplers(n, samplers) {
      __glGenObject(n, samplers, 'createSampler', GL.samplers
        );
    }

  function _emscripten_glGenTextures(n, textures) {
      __glGenObject(n, textures, 'createTexture', GL.textures
        );
    }

  function _emscripten_glGenTransformFeedbacks(n, ids) {
      __glGenObject(n, ids, 'createTransformFeedback', GL.transformFeedbacks
        );
    }

  function _emscripten_glGenVertexArrays(n, arrays) {
      __glGenObject(n, arrays, 'createVertexArray', GL.vaos
        );
    }

  function _emscripten_glGenVertexArraysOES(n, arrays) {
      __glGenObject(n, arrays, 'createVertexArray', GL.vaos
        );
    }

  function _emscripten_glGenerateMipmap(x0) { GLctx['generateMipmap'](x0) }

  function _emscripten_glGetActiveAttrib(program, index, bufSize, length, size, type, name) {
      program = GL.programs[program];
      var info = GLctx.getActiveAttrib(program, index);
      if (!info) return; // If an error occurs, nothing will be written to length, size and type and name.
  
      var numBytesWrittenExclNull = (bufSize > 0 && name) ? stringToUTF8(info.name, name, bufSize) : 0;
      if (length) HEAP32[((length)>>2)]=numBytesWrittenExclNull;
      if (size) HEAP32[((size)>>2)]=info.size;
      if (type) HEAP32[((type)>>2)]=info.type;
    }

  function _emscripten_glGetActiveUniform(program, index, bufSize, length, size, type, name) {
      program = GL.programs[program];
      var info = GLctx.getActiveUniform(program, index);
      if (!info) return; // If an error occurs, nothing will be written to length, size, type and name.
  
      var numBytesWrittenExclNull = (bufSize > 0 && name) ? stringToUTF8(info.name, name, bufSize) : 0;
      if (length) HEAP32[((length)>>2)]=numBytesWrittenExclNull;
      if (size) HEAP32[((size)>>2)]=info.size;
      if (type) HEAP32[((type)>>2)]=info.type;
    }

  function _emscripten_glGetActiveUniformBlockName(program, uniformBlockIndex, bufSize, length, uniformBlockName) {
      program = GL.programs[program];
  
      var result = GLctx['getActiveUniformBlockName'](program, uniformBlockIndex);
      if (!result) return; // If an error occurs, nothing will be written to uniformBlockName or length.
      if (uniformBlockName && bufSize > 0) {
        var numBytesWrittenExclNull = stringToUTF8(result, uniformBlockName, bufSize);
        if (length) HEAP32[((length)>>2)]=numBytesWrittenExclNull;
      } else {
        if (length) HEAP32[((length)>>2)]=0;
      }
    }

  function _emscripten_glGetActiveUniformBlockiv(program, uniformBlockIndex, pname, params) {
      if (!params) {
        // GLES2 specification does not specify how to behave if params is a null pointer. Since calling this function does not make sense
        // if params == null, issue a GL error to notify user about it.
        GL.recordError(0x0501 /* GL_INVALID_VALUE */);
        return;
      }
      program = GL.programs[program];
  
      switch(pname) {
        case 0x8A41: /* GL_UNIFORM_BLOCK_NAME_LENGTH */
          var name = GLctx['getActiveUniformBlockName'](program, uniformBlockIndex);
          HEAP32[((params)>>2)]=name.length+1;
          return;
        default:
          var result = GLctx['getActiveUniformBlockParameter'](program, uniformBlockIndex, pname);
          if (!result) return; // If an error occurs, nothing will be written to params.
          if (typeof result == 'number') {
            HEAP32[((params)>>2)]=result;
          } else {
            for (var i = 0; i < result.length; i++) {
              HEAP32[(((params)+(i*4))>>2)]=result[i];
            }
          }
      }
    }

  function _emscripten_glGetActiveUniformsiv(program, uniformCount, uniformIndices, pname, params) {
      if (!params) {
        // GLES2 specification does not specify how to behave if params is a null pointer. Since calling this function does not make sense
        // if params == null, issue a GL error to notify user about it.
        GL.recordError(0x0501 /* GL_INVALID_VALUE */);
        return;
      }
      if (uniformCount > 0 && uniformIndices == 0) {
        GL.recordError(0x0501 /* GL_INVALID_VALUE */);
        return;
      }
      program = GL.programs[program];
      var ids = [];
      for (var i = 0; i < uniformCount; i++) {
        ids.push(HEAP32[(((uniformIndices)+(i*4))>>2)]);
      }
  
      var result = GLctx['getActiveUniforms'](program, ids, pname);
      if (!result) return; // GL spec: If an error is generated, nothing is written out to params.
  
      var len = result.length;
      for (var i = 0; i < len; i++) {
        HEAP32[(((params)+(i*4))>>2)]=result[i];
      }
    }

  function _emscripten_glGetAttachedShaders(program, maxCount, count, shaders) {
      var result = GLctx.getAttachedShaders(GL.programs[program]);
      var len = result.length;
      if (len > maxCount) {
        len = maxCount;
      }
      HEAP32[((count)>>2)]=len;
      for (var i = 0; i < len; ++i) {
        var id = GL.shaders.indexOf(result[i]);
        HEAP32[(((shaders)+(i*4))>>2)]=id;
      }
    }

  function _emscripten_glGetAttribLocation(program, name) {
      return GLctx.getAttribLocation(GL.programs[program], UTF8ToString(name));
    }

  
  function emscriptenWebGLGet(name_, p, type) {
      // Guard against user passing a null pointer.
      // Note that GLES2 spec does not say anything about how passing a null pointer should be treated.
      // Testing on desktop core GL 3, the application crashes on glGetIntegerv to a null pointer, but
      // better to report an error instead of doing anything random.
      if (!p) {
        GL.recordError(0x0501 /* GL_INVALID_VALUE */);
        return;
      }
      var ret = undefined;
      switch(name_) { // Handle a few trivial GLES values
        case 0x8DFA: // GL_SHADER_COMPILER
          ret = 1;
          break;
        case 0x8DF8: // GL_SHADER_BINARY_FORMATS
          if (type != 0 && type != 1) {
            GL.recordError(0x0500); // GL_INVALID_ENUM
          }
          return; // Do not write anything to the out pointer, since no binary formats are supported.
        case 0x87FE: // GL_NUM_PROGRAM_BINARY_FORMATS
        case 0x8DF9: // GL_NUM_SHADER_BINARY_FORMATS
          ret = 0;
          break;
        case 0x86A2: // GL_NUM_COMPRESSED_TEXTURE_FORMATS
          // WebGL doesn't have GL_NUM_COMPRESSED_TEXTURE_FORMATS (it's obsolete since GL_COMPRESSED_TEXTURE_FORMATS returns a JS array that can be queried for length),
          // so implement it ourselves to allow C++ GLES2 code get the length.
          var formats = GLctx.getParameter(0x86A3 /*GL_COMPRESSED_TEXTURE_FORMATS*/);
          ret = formats ? formats.length : 0;
          break;
        case 0x821D: // GL_NUM_EXTENSIONS
          if (GL.currentContext.version < 2) {
            GL.recordError(0x0502 /* GL_INVALID_OPERATION */); // Calling GLES3/WebGL2 function with a GLES2/WebGL1 context
            return;
          }
          var exts = GLctx.getSupportedExtensions();
          ret = 2 * exts.length; // each extension is duplicated, first in unprefixed WebGL form, and then a second time with "GL_" prefix.
          break;
        case 0x821B: // GL_MAJOR_VERSION
        case 0x821C: // GL_MINOR_VERSION
          if (GL.currentContext.version < 2) {
            GL.recordError(0x0500); // GL_INVALID_ENUM
            return;
          }
          ret = name_ == 0x821B ? 3 : 0; // return version 3.0
          break;
      }
  
      if (ret === undefined) {
        var result = GLctx.getParameter(name_);
        switch (typeof(result)) {
          case "number":
            ret = result;
            break;
          case "boolean":
            ret = result ? 1 : 0;
            break;
          case "string":
            GL.recordError(0x0500); // GL_INVALID_ENUM
            return;
          case "object":
            if (result === null) {
              // null is a valid result for some (e.g., which buffer is bound - perhaps nothing is bound), but otherwise
              // can mean an invalid name_, which we need to report as an error
              switch(name_) {
                case 0x8894: // ARRAY_BUFFER_BINDING
                case 0x8B8D: // CURRENT_PROGRAM
                case 0x8895: // ELEMENT_ARRAY_BUFFER_BINDING
                case 0x8CA6: // FRAMEBUFFER_BINDING
                case 0x8CA7: // RENDERBUFFER_BINDING
                case 0x8069: // TEXTURE_BINDING_2D
                case 0x85B5: // WebGL 2 GL_VERTEX_ARRAY_BINDING, or WebGL 1 extension OES_vertex_array_object GL_VERTEX_ARRAY_BINDING_OES
                case 0x8919: // GL_SAMPLER_BINDING
                case 0x8E25: // GL_TRANSFORM_FEEDBACK_BINDING
                case 0x8514: { // TEXTURE_BINDING_CUBE_MAP
                  ret = 0;
                  break;
                }
                default: {
                  GL.recordError(0x0500); // GL_INVALID_ENUM
                  return;
                }
              }
            } else if (result instanceof Float32Array ||
                       result instanceof Uint32Array ||
                       result instanceof Int32Array ||
                       result instanceof Array) {
              for (var i = 0; i < result.length; ++i) {
                switch (type) {
                  case 0: HEAP32[(((p)+(i*4))>>2)]=result[i]; break;
                  case 2: HEAPF32[(((p)+(i*4))>>2)]=result[i]; break;
                  case 4: HEAP8[(((p)+(i))>>0)]=result[i] ? 1 : 0; break;
                }
              }
              return;
            } else {
              try {
                ret = result.name | 0;
              } catch(e) {
                GL.recordError(0x0500); // GL_INVALID_ENUM
                err('GL_INVALID_ENUM in glGet' + type + 'v: Unknown object returned from WebGL getParameter(' + name_ + ')! (error: ' + e + ')');
                return;
              }
            }
            break;
          default:
            GL.recordError(0x0500); // GL_INVALID_ENUM
            err('GL_INVALID_ENUM in glGet' + type + 'v: Native code calling glGet' + type + 'v(' + name_ + ') and it returns ' + result + ' of type ' + typeof(result) + '!');
            return;
        }
      }
  
      switch (type) {
        case 1: (tempI64 = [ret>>>0,(tempDouble=ret,(+(Math_abs(tempDouble))) >= 1.0 ? (tempDouble > 0.0 ? ((Math_min((+(Math_floor((tempDouble)/4294967296.0))), 4294967295.0))|0)>>>0 : (~~((+(Math_ceil((tempDouble - +(((~~(tempDouble)))>>>0))/4294967296.0)))))>>>0) : 0)],HEAP32[((p)>>2)]=tempI64[0],HEAP32[(((p)+(4))>>2)]=tempI64[1]);    break;
        case 0: HEAP32[((p)>>2)]=ret;    break;
        case 2:   HEAPF32[((p)>>2)]=ret;  break;
        case 4: HEAP8[((p)>>0)]=ret ? 1 : 0; break;
      }
    }function _emscripten_glGetBooleanv(name_, p) {
      emscriptenWebGLGet(name_, p, 4);
    }

  function _emscripten_glGetBufferParameteri64v(target, value, data) {
      if (!data) {
        // GLES2 specification does not specify how to behave if data is a null pointer. Since calling this function does not make sense
        // if data == null, issue a GL error to notify user about it.
        GL.recordError(0x0501 /* GL_INVALID_VALUE */);
        return;
      }
      (tempI64 = [GLctx.getBufferParameter(target, value)>>>0,(tempDouble=GLctx.getBufferParameter(target, value),(+(Math_abs(tempDouble))) >= 1.0 ? (tempDouble > 0.0 ? ((Math_min((+(Math_floor((tempDouble)/4294967296.0))), 4294967295.0))|0)>>>0 : (~~((+(Math_ceil((tempDouble - +(((~~(tempDouble)))>>>0))/4294967296.0)))))>>>0) : 0)],HEAP32[((data)>>2)]=tempI64[0],HEAP32[(((data)+(4))>>2)]=tempI64[1]);
    }

  function _emscripten_glGetBufferParameteriv(target, value, data) {
      if (!data) {
        // GLES2 specification does not specify how to behave if data is a null pointer. Since calling this function does not make sense
        // if data == null, issue a GL error to notify user about it.
        GL.recordError(0x0501 /* GL_INVALID_VALUE */);
        return;
      }
      HEAP32[((data)>>2)]=GLctx.getBufferParameter(target, value);
    }

  function _emscripten_glGetBufferPointerv(
  ) {
  err('missing function: emscripten_glGetBufferPointerv'); abort(-1);
  }

  function _emscripten_glGetError() {
      // First return any GL error generated by the emscripten library_webgl.js interop layer.
      if (GL.lastError) {
        var error = GL.lastError;
        GL.lastError = 0/*GL_NO_ERROR*/;
        return error;
      } else
      { // If there were none, return the GL error from the browser GL context.
        return GLctx.getError();
      }
    }

  function _emscripten_glGetFloatv(name_, p) {
      emscriptenWebGLGet(name_, p, 2);
    }

  function _emscripten_glGetFragDataLocation(program, name) {
      return GLctx['getFragDataLocation'](GL.programs[program], UTF8ToString(name));
    }

  function _emscripten_glGetFramebufferAttachmentParameteriv(target, attachment, pname, params) {
      var result = GLctx.getFramebufferAttachmentParameter(target, attachment, pname);
      if (result instanceof WebGLRenderbuffer ||
          result instanceof WebGLTexture) {
        result = result.name | 0;
      }
      HEAP32[((params)>>2)]=result;
    }

  
  function emscriptenWebGLGetIndexed(target, index, data, type) {
      if (!data) {
        // GLES2 specification does not specify how to behave if data is a null pointer. Since calling this function does not make sense
        // if data == null, issue a GL error to notify user about it.
        GL.recordError(0x0501 /* GL_INVALID_VALUE */);
        return;
      }
      var result = GLctx['getIndexedParameter'](target, index);
      var ret;
      switch (typeof result) {
        case 'boolean':
          ret = result ? 1 : 0;
          break;
        case 'number':
          ret = result;
          break;
        case 'object':
          if (result === null) {
            switch (target) {
              case 0x8C8F: // TRANSFORM_FEEDBACK_BUFFER_BINDING
              case 0x8A28: // UNIFORM_BUFFER_BINDING
                ret = 0;
                break;
              default: {
                GL.recordError(0x0500); // GL_INVALID_ENUM
                return;
              }
            }
          } else if (result instanceof WebGLBuffer) {
            ret = result.name | 0;
          } else {
            GL.recordError(0x0500); // GL_INVALID_ENUM
            return;
          }
          break;
        default:
          GL.recordError(0x0500); // GL_INVALID_ENUM
          return;
      }
  
      switch (type) {
        case 1: (tempI64 = [ret>>>0,(tempDouble=ret,(+(Math_abs(tempDouble))) >= 1.0 ? (tempDouble > 0.0 ? ((Math_min((+(Math_floor((tempDouble)/4294967296.0))), 4294967295.0))|0)>>>0 : (~~((+(Math_ceil((tempDouble - +(((~~(tempDouble)))>>>0))/4294967296.0)))))>>>0) : 0)],HEAP32[((data)>>2)]=tempI64[0],HEAP32[(((data)+(4))>>2)]=tempI64[1]); break;
        case 0: HEAP32[((data)>>2)]=ret; break;
        case 2: HEAPF32[((data)>>2)]=ret; break;
        case 4: HEAP8[((data)>>0)]=ret ? 1 : 0; break;
        default: throw 'internal emscriptenWebGLGetIndexed() error, bad type: ' + type;
      }
    }function _emscripten_glGetInteger64i_v(target, index, data) {
      emscriptenWebGLGetIndexed(target, index, data, 1);
    }

  function _emscripten_glGetInteger64v(name_, p) {
      emscriptenWebGLGet(name_, p, 1);
    }

  function _emscripten_glGetIntegeri_v(target, index, data) {
      emscriptenWebGLGetIndexed(target, index, data, 0);
    }

  function _emscripten_glGetIntegerv(name_, p) {
      emscriptenWebGLGet(name_, p, 0);
    }

  function _emscripten_glGetInternalformativ(target, internalformat, pname, bufSize, params) {
      if (bufSize < 0) {
        GL.recordError(0x0501 /* GL_INVALID_VALUE */);
        return;
      }
      if (!params) {
        // GLES3 specification does not specify how to behave if values is a null pointer. Since calling this function does not make sense
        // if values == null, issue a GL error to notify user about it.
        GL.recordError(0x0501 /* GL_INVALID_VALUE */);
        return;
      }
      var ret = GLctx['getInternalformatParameter'](target, internalformat, pname);
      if (ret === null) return;
      for (var i = 0; i < ret.length && i < bufSize; ++i) {
        HEAP32[(((params)+(i))>>2)]=ret[i];
      }
    }

  function _emscripten_glGetProgramBinary(program, bufSize, length, binaryFormat, binary) {
      GL.recordError(0x0502/*GL_INVALID_OPERATION*/);
    }

  function _emscripten_glGetProgramInfoLog(program, maxLength, length, infoLog) {
      var log = GLctx.getProgramInfoLog(GL.programs[program]);
      if (log === null) log = '(unknown error)';
      var numBytesWrittenExclNull = (maxLength > 0 && infoLog) ? stringToUTF8(log, infoLog, maxLength) : 0;
      if (length) HEAP32[((length)>>2)]=numBytesWrittenExclNull;
    }

  function _emscripten_glGetProgramiv(program, pname, p) {
      if (!p) {
        // GLES2 specification does not specify how to behave if p is a null pointer. Since calling this function does not make sense
        // if p == null, issue a GL error to notify user about it.
        GL.recordError(0x0501 /* GL_INVALID_VALUE */);
        return;
      }
  
      if (program >= GL.counter) {
        GL.recordError(0x0501 /* GL_INVALID_VALUE */);
        return;
      }
  
      var ptable = GL.programInfos[program];
      if (!ptable) {
        GL.recordError(0x0502 /* GL_INVALID_OPERATION */);
        return;
      }
  
      if (pname == 0x8B84) { // GL_INFO_LOG_LENGTH
        var log = GLctx.getProgramInfoLog(GL.programs[program]);
        if (log === null) log = '(unknown error)';
        HEAP32[((p)>>2)]=log.length + 1;
      } else if (pname == 0x8B87 /* GL_ACTIVE_UNIFORM_MAX_LENGTH */) {
        HEAP32[((p)>>2)]=ptable.maxUniformLength;
      } else if (pname == 0x8B8A /* GL_ACTIVE_ATTRIBUTE_MAX_LENGTH */) {
        if (ptable.maxAttributeLength == -1) {
          program = GL.programs[program];
          var numAttribs = GLctx.getProgramParameter(program, 0x8B89/*GL_ACTIVE_ATTRIBUTES*/);
          ptable.maxAttributeLength = 0; // Spec says if there are no active attribs, 0 must be returned.
          for (var i = 0; i < numAttribs; ++i) {
            var activeAttrib = GLctx.getActiveAttrib(program, i);
            ptable.maxAttributeLength = Math.max(ptable.maxAttributeLength, activeAttrib.name.length+1);
          }
        }
        HEAP32[((p)>>2)]=ptable.maxAttributeLength;
      } else if (pname == 0x8A35 /* GL_ACTIVE_UNIFORM_BLOCK_MAX_NAME_LENGTH */) {
        if (ptable.maxUniformBlockNameLength == -1) {
          program = GL.programs[program];
          var numBlocks = GLctx.getProgramParameter(program, 0x8A36/*GL_ACTIVE_UNIFORM_BLOCKS*/);
          ptable.maxUniformBlockNameLength = 0;
          for (var i = 0; i < numBlocks; ++i) {
            var activeBlockName = GLctx.getActiveUniformBlockName(program, i);
            ptable.maxUniformBlockNameLength = Math.max(ptable.maxUniformBlockNameLength, activeBlockName.length+1);
          }
        }
        HEAP32[((p)>>2)]=ptable.maxUniformBlockNameLength;
      } else {
        HEAP32[((p)>>2)]=GLctx.getProgramParameter(GL.programs[program], pname);
      }
    }

  function _emscripten_glGetQueryObjecti64vEXT(id, pname, params) {
      if (!params) {
        // GLES2 specification does not specify how to behave if params is a null pointer. Since calling this function does not make sense
        // if p == null, issue a GL error to notify user about it.
        GL.recordError(0x0501 /* GL_INVALID_VALUE */);
        return;
      }
      var query = GL.timerQueriesEXT[id];
      var param = GLctx.disjointTimerQueryExt['getQueryObjectEXT'](query, pname);
      var ret;
      if (typeof param == 'boolean') {
        ret = param ? 1 : 0;
      } else {
        ret = param;
      }
      (tempI64 = [ret>>>0,(tempDouble=ret,(+(Math_abs(tempDouble))) >= 1.0 ? (tempDouble > 0.0 ? ((Math_min((+(Math_floor((tempDouble)/4294967296.0))), 4294967295.0))|0)>>>0 : (~~((+(Math_ceil((tempDouble - +(((~~(tempDouble)))>>>0))/4294967296.0)))))>>>0) : 0)],HEAP32[((params)>>2)]=tempI64[0],HEAP32[(((params)+(4))>>2)]=tempI64[1]);
    }

  function _emscripten_glGetQueryObjectivEXT(id, pname, params) {
      if (!params) {
        // GLES2 specification does not specify how to behave if params is a null pointer. Since calling this function does not make sense
        // if p == null, issue a GL error to notify user about it.
        GL.recordError(0x0501 /* GL_INVALID_VALUE */);
        return;
      }
      var query = GL.timerQueriesEXT[id];
      var param = GLctx.disjointTimerQueryExt['getQueryObjectEXT'](query, pname);
      var ret;
      if (typeof param == 'boolean') {
        ret = param ? 1 : 0;
      } else {
        ret = param;
      }
      HEAP32[((params)>>2)]=ret;
    }

  function _emscripten_glGetQueryObjectui64vEXT(id, pname, params) {
      if (!params) {
        // GLES2 specification does not specify how to behave if params is a null pointer. Since calling this function does not make sense
        // if p == null, issue a GL error to notify user about it.
        GL.recordError(0x0501 /* GL_INVALID_VALUE */);
        return;
      }
      var query = GL.timerQueriesEXT[id];
      var param = GLctx.disjointTimerQueryExt['getQueryObjectEXT'](query, pname);
      var ret;
      if (typeof param == 'boolean') {
        ret = param ? 1 : 0;
      } else {
        ret = param;
      }
      (tempI64 = [ret>>>0,(tempDouble=ret,(+(Math_abs(tempDouble))) >= 1.0 ? (tempDouble > 0.0 ? ((Math_min((+(Math_floor((tempDouble)/4294967296.0))), 4294967295.0))|0)>>>0 : (~~((+(Math_ceil((tempDouble - +(((~~(tempDouble)))>>>0))/4294967296.0)))))>>>0) : 0)],HEAP32[((params)>>2)]=tempI64[0],HEAP32[(((params)+(4))>>2)]=tempI64[1]);
    }

  function _emscripten_glGetQueryObjectuiv(id, pname, params) {
      if (!params) {
        // GLES2 specification does not specify how to behave if params is a null pointer. Since calling this function does not make sense
        // if p == null, issue a GL error to notify user about it.
        GL.recordError(0x0501 /* GL_INVALID_VALUE */);
        return;
      }
      var query = GL.queries[id];
      var param = GLctx['getQueryParameter'](query, pname);
      var ret;
      if (typeof param == 'boolean') {
        ret = param ? 1 : 0;
      } else {
        ret = param;
      }
      HEAP32[((params)>>2)]=ret;
    }

  function _emscripten_glGetQueryObjectuivEXT(id, pname, params) {
      if (!params) {
        // GLES2 specification does not specify how to behave if params is a null pointer. Since calling this function does not make sense
        // if p == null, issue a GL error to notify user about it.
        GL.recordError(0x0501 /* GL_INVALID_VALUE */);
        return;
      }
      var query = GL.timerQueriesEXT[id];
      var param = GLctx.disjointTimerQueryExt['getQueryObjectEXT'](query, pname);
      var ret;
      if (typeof param == 'boolean') {
        ret = param ? 1 : 0;
      } else {
        ret = param;
      }
      HEAP32[((params)>>2)]=ret;
    }

  function _emscripten_glGetQueryiv(target, pname, params) {
      if (!params) {
        // GLES2 specification does not specify how to behave if params is a null pointer. Since calling this function does not make sense
        // if p == null, issue a GL error to notify user about it.
        GL.recordError(0x0501 /* GL_INVALID_VALUE */);
        return;
      }
      HEAP32[((params)>>2)]=GLctx['getQuery'](target, pname);
    }

  function _emscripten_glGetQueryivEXT(target, pname, params) {
      if (!params) {
        // GLES2 specification does not specify how to behave if params is a null pointer. Since calling this function does not make sense
        // if p == null, issue a GL error to notify user about it.
        GL.recordError(0x0501 /* GL_INVALID_VALUE */);
        return;
      }
      HEAP32[((params)>>2)]=GLctx.disjointTimerQueryExt['getQueryEXT'](target, pname);
    }

  function _emscripten_glGetRenderbufferParameteriv(target, pname, params) {
      if (!params) {
        // GLES2 specification does not specify how to behave if params is a null pointer. Since calling this function does not make sense
        // if params == null, issue a GL error to notify user about it.
        GL.recordError(0x0501 /* GL_INVALID_VALUE */);
        return;
      }
      HEAP32[((params)>>2)]=GLctx.getRenderbufferParameter(target, pname);
    }

  function _emscripten_glGetSamplerParameterfv(sampler, pname, params) {
      if (!params) {
        // GLES3 specification does not specify how to behave if params is a null pointer. Since calling this function does not make sense
        // if p == null, issue a GL error to notify user about it.
        GL.recordError(0x0501 /* GL_INVALID_VALUE */);
        return;
      }
      sampler = GL.samplers[sampler];
      HEAPF32[((params)>>2)]=GLctx['getSamplerParameter'](sampler, pname);
    }

  function _emscripten_glGetSamplerParameteriv(sampler, pname, params) {
      if (!params) {
        // GLES3 specification does not specify how to behave if params is a null pointer. Since calling this function does not make sense
        // if p == null, issue a GL error to notify user about it.
        GL.recordError(0x0501 /* GL_INVALID_VALUE */);
        return;
      }
      sampler = GL.samplers[sampler];
      HEAP32[((params)>>2)]=GLctx['getSamplerParameter'](sampler, pname);
    }

  function _emscripten_glGetShaderInfoLog(shader, maxLength, length, infoLog) {
      var log = GLctx.getShaderInfoLog(GL.shaders[shader]);
      if (log === null) log = '(unknown error)';
      var numBytesWrittenExclNull = (maxLength > 0 && infoLog) ? stringToUTF8(log, infoLog, maxLength) : 0;
      if (length) HEAP32[((length)>>2)]=numBytesWrittenExclNull;
    }

  function _emscripten_glGetShaderPrecisionFormat(shaderType, precisionType, range, precision) {
      var result = GLctx.getShaderPrecisionFormat(shaderType, precisionType);
      HEAP32[((range)>>2)]=result.rangeMin;
      HEAP32[(((range)+(4))>>2)]=result.rangeMax;
      HEAP32[((precision)>>2)]=result.precision;
    }

  function _emscripten_glGetShaderSource(shader, bufSize, length, source) {
      var result = GLctx.getShaderSource(GL.shaders[shader]);
      if (!result) return; // If an error occurs, nothing will be written to length or source.
      var numBytesWrittenExclNull = (bufSize > 0 && source) ? stringToUTF8(result, source, bufSize) : 0;
      if (length) HEAP32[((length)>>2)]=numBytesWrittenExclNull;
    }

  function _emscripten_glGetShaderiv(shader, pname, p) {
      if (!p) {
        // GLES2 specification does not specify how to behave if p is a null pointer. Since calling this function does not make sense
        // if p == null, issue a GL error to notify user about it.
        GL.recordError(0x0501 /* GL_INVALID_VALUE */);
        return;
      }
      if (pname == 0x8B84) { // GL_INFO_LOG_LENGTH
        var log = GLctx.getShaderInfoLog(GL.shaders[shader]);
        if (log === null) log = '(unknown error)';
        HEAP32[((p)>>2)]=log.length + 1;
      } else if (pname == 0x8B88) { // GL_SHADER_SOURCE_LENGTH
        var source = GLctx.getShaderSource(GL.shaders[shader]);
        var sourceLength = (source === null || source.length == 0) ? 0 : source.length + 1;
        HEAP32[((p)>>2)]=sourceLength;
      } else {
        HEAP32[((p)>>2)]=GLctx.getShaderParameter(GL.shaders[shader], pname);
      }
    }

  
  function stringToNewUTF8(jsString) {
      var length = lengthBytesUTF8(jsString)+1;
      var cString = _malloc(length);
      stringToUTF8(jsString, cString, length);
      return cString;
    }function _emscripten_glGetString(name_) {
      if (GL.stringCache[name_]) return GL.stringCache[name_];
      var ret;
      switch(name_) {
        case 0x1F03 /* GL_EXTENSIONS */:
          var exts = GLctx.getSupportedExtensions();
          var gl_exts = [];
          for (var i = 0; i < exts.length; ++i) {
            gl_exts.push(exts[i]);
            gl_exts.push("GL_" + exts[i]);
          }
          ret = stringToNewUTF8(gl_exts.join(' '));
          break;
        case 0x1F00 /* GL_VENDOR */:
        case 0x1F01 /* GL_RENDERER */:
        case 0x9245 /* UNMASKED_VENDOR_WEBGL */:
        case 0x9246 /* UNMASKED_RENDERER_WEBGL */:
          var s = GLctx.getParameter(name_);
          if (!s) {
            GL.recordError(0x0500/*GL_INVALID_ENUM*/);
          }
          ret = stringToNewUTF8(s);
          break;
  
        case 0x1F02 /* GL_VERSION */:
          var glVersion = GLctx.getParameter(GLctx.VERSION);
          // return GLES version string corresponding to the version of the WebGL context
          if (GL.currentContext.version >= 2) glVersion = 'OpenGL ES 3.0 (' + glVersion + ')';
          else
          {
            glVersion = 'OpenGL ES 2.0 (' + glVersion + ')';
          }
          ret = stringToNewUTF8(glVersion);
          break;
        case 0x8B8C /* GL_SHADING_LANGUAGE_VERSION */:
          var glslVersion = GLctx.getParameter(GLctx.SHADING_LANGUAGE_VERSION);
          // extract the version number 'N.M' from the string 'WebGL GLSL ES N.M ...'
          var ver_re = /^WebGL GLSL ES ([0-9]\.[0-9][0-9]?)(?:$| .*)/;
          var ver_num = glslVersion.match(ver_re);
          if (ver_num !== null) {
            if (ver_num[1].length == 3) ver_num[1] = ver_num[1] + '0'; // ensure minor version has 2 digits
            glslVersion = 'OpenGL ES GLSL ES ' + ver_num[1] + ' (' + glslVersion + ')';
          }
          ret = stringToNewUTF8(glslVersion);
          break;
        default:
          GL.recordError(0x0500/*GL_INVALID_ENUM*/);
          return 0;
      }
      GL.stringCache[name_] = ret;
      return ret;
    }

  function _emscripten_glGetStringi(name, index) {
      if (GL.currentContext.version < 2) {
        GL.recordError(0x0502 /* GL_INVALID_OPERATION */); // Calling GLES3/WebGL2 function with a GLES2/WebGL1 context
        return 0;
      }
      var stringiCache = GL.stringiCache[name];
      if (stringiCache) {
        if (index < 0 || index >= stringiCache.length) {
          GL.recordError(0x0501/*GL_INVALID_VALUE*/);
          return 0;
        }
        return stringiCache[index];
      }
      switch(name) {
        case 0x1F03 /* GL_EXTENSIONS */:
          var exts = GLctx.getSupportedExtensions();
          var gl_exts = [];
          for (var i = 0; i < exts.length; ++i) {
            gl_exts.push(stringToNewUTF8(exts[i]));
            // each extension is duplicated, first in unprefixed WebGL form, and then a second time with "GL_" prefix.
            gl_exts.push(stringToNewUTF8('GL_' + exts[i]));
          }
          stringiCache = GL.stringiCache[name] = gl_exts;
          if (index < 0 || index >= stringiCache.length) {
            GL.recordError(0x0501/*GL_INVALID_VALUE*/);
            return 0;
          }
          return stringiCache[index];
        default:
          GL.recordError(0x0500/*GL_INVALID_ENUM*/);
          return 0;
      }
    }

  function _emscripten_glGetSynciv(sync, pname, bufSize, length, values) {
      if (bufSize < 0) {
        // GLES3 specification does not specify how to behave if bufSize < 0, however in the spec wording for glGetInternalformativ, it does say that GL_INVALID_VALUE should be raised,
        // so raise GL_INVALID_VALUE here as well.
        GL.recordError(0x0501 /* GL_INVALID_VALUE */);
        return;
      }
      if (!values) {
        // GLES3 specification does not specify how to behave if values is a null pointer. Since calling this function does not make sense
        // if values == null, issue a GL error to notify user about it.
        GL.recordError(0x0501 /* GL_INVALID_VALUE */);
        return;
      }
      var ret = GLctx.getSyncParameter(GL.syncs[sync], pname);
      HEAP32[((length)>>2)]=ret;
      if (ret !== null && length) HEAP32[((length)>>2)]=1; // Report a single value outputted.
    }

  function _emscripten_glGetTexParameterfv(target, pname, params) {
      if (!params) {
        // GLES2 specification does not specify how to behave if params is a null pointer. Since calling this function does not make sense
        // if p == null, issue a GL error to notify user about it.
        GL.recordError(0x0501 /* GL_INVALID_VALUE */);
        return;
      }
      HEAPF32[((params)>>2)]=GLctx.getTexParameter(target, pname);
    }

  function _emscripten_glGetTexParameteriv(target, pname, params) {
      if (!params) {
        // GLES2 specification does not specify how to behave if params is a null pointer. Since calling this function does not make sense
        // if p == null, issue a GL error to notify user about it.
        GL.recordError(0x0501 /* GL_INVALID_VALUE */);
        return;
      }
      HEAP32[((params)>>2)]=GLctx.getTexParameter(target, pname);
    }

  function _emscripten_glGetTransformFeedbackVarying(program, index, bufSize, length, size, type, name) {
      program = GL.programs[program];
      var info = GLctx['getTransformFeedbackVarying'](program, index);
      if (!info) return; // If an error occurred, the return parameters length, size, type and name will be unmodified.
  
      if (name && bufSize > 0) {
        var numBytesWrittenExclNull = stringToUTF8(info.name, name, bufSize);
        if (length) HEAP32[((length)>>2)]=numBytesWrittenExclNull;
      } else {
        if (length) HEAP32[((length)>>2)]=0;
      }
  
      if (size) HEAP32[((size)>>2)]=info.size;
      if (type) HEAP32[((type)>>2)]=info.type;
    }

  function _emscripten_glGetUniformBlockIndex(program, uniformBlockName) {
      return GLctx['getUniformBlockIndex'](GL.programs[program], UTF8ToString(uniformBlockName));
    }

  function _emscripten_glGetUniformIndices(program, uniformCount, uniformNames, uniformIndices) {
      if (!uniformIndices) {
        // GLES2 specification does not specify how to behave if uniformIndices is a null pointer. Since calling this function does not make sense
        // if uniformIndices == null, issue a GL error to notify user about it.
        GL.recordError(0x0501 /* GL_INVALID_VALUE */);
        return;
      }
      if (uniformCount > 0 && (uniformNames == 0 || uniformIndices == 0)) {
        GL.recordError(0x0501 /* GL_INVALID_VALUE */);
        return;
      }
      program = GL.programs[program];
      var names = [];
      for (var i = 0; i < uniformCount; i++)
        names.push(UTF8ToString(HEAP32[(((uniformNames)+(i*4))>>2)]));
  
      var result = GLctx['getUniformIndices'](program, names);
      if (!result) return; // GL spec: If an error is generated, nothing is written out to uniformIndices.
  
      var len = result.length;
      for (var i = 0; i < len; i++) {
        HEAP32[(((uniformIndices)+(i*4))>>2)]=result[i];
      }
    }

  function _emscripten_glGetUniformLocation(program, name) {
      name = UTF8ToString(name);
  
      var arrayIndex = 0;
      // If user passed an array accessor "[index]", parse the array index off the accessor.
      if (name[name.length - 1] == ']') {
        var leftBrace = name.lastIndexOf('[');
        arrayIndex = name[leftBrace+1] != ']' ? parseInt(name.slice(leftBrace + 1)) : 0; // "index]", parseInt will ignore the ']' at the end; but treat "foo[]" as "foo[0]"
        name = name.slice(0, leftBrace);
      }
  
      var uniformInfo = GL.programInfos[program] && GL.programInfos[program].uniforms[name]; // returns pair [ dimension_of_uniform_array, uniform_location ]
      if (uniformInfo && arrayIndex >= 0 && arrayIndex < uniformInfo[0]) { // Check if user asked for an out-of-bounds element, i.e. for 'vec4 colors[3];' user could ask for 'colors[10]' which should return -1.
        return uniformInfo[1] + arrayIndex;
      } else {
        return -1;
      }
    }

  
  function emscriptenWebGLGetUniform(program, location, params, type) {
      if (!params) {
        // GLES2 specification does not specify how to behave if params is a null pointer. Since calling this function does not make sense
        // if params == null, issue a GL error to notify user about it.
        GL.recordError(0x0501 /* GL_INVALID_VALUE */);
        return;
      }
      var data = GLctx.getUniform(GL.programs[program], GL.uniforms[location]);
      if (typeof data == 'number' || typeof data == 'boolean') {
        switch (type) {
          case 0: HEAP32[((params)>>2)]=data; break;
          case 2: HEAPF32[((params)>>2)]=data; break;
          default: throw 'internal emscriptenWebGLGetUniform() error, bad type: ' + type;
        }
      } else {
        for (var i = 0; i < data.length; i++) {
          switch (type) {
            case 0: HEAP32[(((params)+(i*4))>>2)]=data[i]; break;
            case 2: HEAPF32[(((params)+(i*4))>>2)]=data[i]; break;
            default: throw 'internal emscriptenWebGLGetUniform() error, bad type: ' + type;
          }
        }
      }
    }function _emscripten_glGetUniformfv(program, location, params) {
      emscriptenWebGLGetUniform(program, location, params, 2);
    }

  function _emscripten_glGetUniformiv(program, location, params) {
      emscriptenWebGLGetUniform(program, location, params, 0);
    }

  function _emscripten_glGetUniformuiv(program, location, params) {
      emscriptenWebGLGetUniform(program, location, params, 0);
    }

  
  function emscriptenWebGLGetVertexAttrib(index, pname, params, type) {
      if (!params) {
        // GLES2 specification does not specify how to behave if params is a null pointer. Since calling this function does not make sense
        // if params == null, issue a GL error to notify user about it.
        GL.recordError(0x0501 /* GL_INVALID_VALUE */);
        return;
      }
      var data = GLctx.getVertexAttrib(index, pname);
      if (pname == 0x889F/*VERTEX_ATTRIB_ARRAY_BUFFER_BINDING*/) {
        HEAP32[((params)>>2)]=data["name"];
      } else if (typeof data == 'number' || typeof data == 'boolean') {
        switch (type) {
          case 0: HEAP32[((params)>>2)]=data; break;
          case 2: HEAPF32[((params)>>2)]=data; break;
          case 5: HEAP32[((params)>>2)]=Math.fround(data); break;
          default: throw 'internal emscriptenWebGLGetVertexAttrib() error, bad type: ' + type;
        }
      } else {
        for (var i = 0; i < data.length; i++) {
          switch (type) {
            case 0: HEAP32[(((params)+(i*4))>>2)]=data[i]; break;
            case 2: HEAPF32[(((params)+(i*4))>>2)]=data[i]; break;
            case 5: HEAP32[(((params)+(i*4))>>2)]=Math.fround(data[i]); break;
            default: throw 'internal emscriptenWebGLGetVertexAttrib() error, bad type: ' + type;
          }
        }
      }
    }function _emscripten_glGetVertexAttribIiv(index, pname, params) {
      // N.B. This function may only be called if the vertex attribute was specified using the function glVertexAttribI4iv(),
      // otherwise the results are undefined. (GLES3 spec 6.1.12)
      emscriptenWebGLGetVertexAttrib(index, pname, params, 0);
    }

  function _emscripten_glGetVertexAttribIuiv(index, pname, params) {
      // N.B. This function may only be called if the vertex attribute was specified using the function glVertexAttribI4iv(),
      // otherwise the results are undefined. (GLES3 spec 6.1.12)
      emscriptenWebGLGetVertexAttrib(index, pname, params, 0);
    }

  function _emscripten_glGetVertexAttribPointerv(index, pname, pointer) {
      if (!pointer) {
        // GLES2 specification does not specify how to behave if pointer is a null pointer. Since calling this function does not make sense
        // if pointer == null, issue a GL error to notify user about it.
        GL.recordError(0x0501 /* GL_INVALID_VALUE */);
        return;
      }
      HEAP32[((pointer)>>2)]=GLctx.getVertexAttribOffset(index, pname);
    }

  function _emscripten_glGetVertexAttribfv(index, pname, params) {
      // N.B. This function may only be called if the vertex attribute was specified using the function glVertexAttrib*f(),
      // otherwise the results are undefined. (GLES3 spec 6.1.12)
      emscriptenWebGLGetVertexAttrib(index, pname, params, 2);
    }

  function _emscripten_glGetVertexAttribiv(index, pname, params) {
      // N.B. This function may only be called if the vertex attribute was specified using the function glVertexAttrib*f(),
      // otherwise the results are undefined. (GLES3 spec 6.1.12)
      emscriptenWebGLGetVertexAttrib(index, pname, params, 5);
    }

  function _emscripten_glHint(x0, x1) { GLctx['hint'](x0, x1) }

  function _emscripten_glInvalidateFramebuffer(target, numAttachments, attachments) {
      var list = __tempFixedLengthArray[numAttachments];
      for (var i = 0; i < numAttachments; i++) {
        list[i] = HEAP32[(((attachments)+(i*4))>>2)];
      }
  
      GLctx['invalidateFramebuffer'](target, list);
    }

  function _emscripten_glInvalidateSubFramebuffer(target, numAttachments, attachments, x, y, width, height) {
      var list = __tempFixedLengthArray[numAttachments];
      for (var i = 0; i < numAttachments; i++) {
        list[i] = HEAP32[(((attachments)+(i*4))>>2)];
      }
  
      GLctx['invalidateSubFramebuffer'](target, list, x, y, width, height);
    }

  function _emscripten_glIsBuffer(buffer) {
      var b = GL.buffers[buffer];
      if (!b) return 0;
      return GLctx.isBuffer(b);
    }

  function _emscripten_glIsEnabled(x0) { return GLctx['isEnabled'](x0) }

  function _emscripten_glIsFramebuffer(framebuffer) {
      var fb = GL.framebuffers[framebuffer];
      if (!fb) return 0;
      return GLctx.isFramebuffer(fb);
    }

  function _emscripten_glIsProgram(program) {
      program = GL.programs[program];
      if (!program) return 0;
      return GLctx.isProgram(program);
    }

  function _emscripten_glIsQuery(id) {
      var query = GL.queries[id];
      if (!query) return 0;
      return GLctx['isQuery'](query);
    }

  function _emscripten_glIsQueryEXT(id) {
      var query = GL.timerQueriesEXT[id];
      if (!query) return 0;
      return GLctx.disjointTimerQueryExt['isQueryEXT'](query);
    }

  function _emscripten_glIsRenderbuffer(renderbuffer) {
      var rb = GL.renderbuffers[renderbuffer];
      if (!rb) return 0;
      return GLctx.isRenderbuffer(rb);
    }

  function _emscripten_glIsSampler(id) {
      var sampler = GL.samplers[id];
      if (!sampler) return 0;
      return GLctx['isSampler'](sampler);
    }

  function _emscripten_glIsShader(shader) {
      var s = GL.shaders[shader];
      if (!s) return 0;
      return GLctx.isShader(s);
    }

  function _emscripten_glIsSync(sync) {
      var sync = GL.syncs[sync];
      if (!sync) return 0;
      return GLctx.isSync(sync);
    }

  function _emscripten_glIsTexture(id) {
      var texture = GL.textures[id];
      if (!texture) return 0;
      return GLctx.isTexture(texture);
    }

  function _emscripten_glIsTransformFeedback(id) {
      return GLctx['isTransformFeedback'](GL.transformFeedbacks[id]);
    }

  function _emscripten_glIsVertexArray(array) {
  
      var vao = GL.vaos[array];
      if (!vao) return 0;
      return GLctx['isVertexArray'](vao);
    }

  function _emscripten_glIsVertexArrayOES(array) {
  
      var vao = GL.vaos[array];
      if (!vao) return 0;
      return GLctx['isVertexArray'](vao);
    }

  function _emscripten_glLineWidth(x0) { GLctx['lineWidth'](x0) }

  function _emscripten_glLinkProgram(program) {
      GLctx.linkProgram(GL.programs[program]);
      GL.populateUniformTable(program);
    }

  function _emscripten_glMapBufferRange(
  ) {
  err('missing function: emscripten_glMapBufferRange'); abort(-1);
  }

  function _emscripten_glPauseTransformFeedback() { GLctx['pauseTransformFeedback']() }

  function _emscripten_glPixelStorei(pname, param) {
      if (pname == 0x0cf5 /* GL_UNPACK_ALIGNMENT */) {
        GL.unpackAlignment = param;
      }
      GLctx.pixelStorei(pname, param);
    }

  function _emscripten_glPolygonOffset(x0, x1) { GLctx['polygonOffset'](x0, x1) }

  function _emscripten_glProgramBinary(program, binaryFormat, binary, length) {
      GL.recordError(0x0500/*GL_INVALID_ENUM*/);
    }

  function _emscripten_glProgramParameteri(program, pname, value) {
      GL.recordError(0x0500/*GL_INVALID_ENUM*/);
    }

  function _emscripten_glQueryCounterEXT(id, target) {
      GLctx.disjointTimerQueryExt['queryCounterEXT'](GL.timerQueriesEXT[id], target);
    }

  function _emscripten_glReadBuffer(x0) { GLctx['readBuffer'](x0) }

  
  
  function __computeUnpackAlignedImageSize(width, height, sizePerPixel, alignment) {
      function roundedToNextMultipleOf(x, y) {
        return (x + y - 1) & -y;
      }
      var plainRowSize = width * sizePerPixel;
      var alignedRowSize = roundedToNextMultipleOf(plainRowSize, alignment);
      return height * alignedRowSize;
    }
  
  var __colorChannelsInGlTextureFormat={6402:1,6403:1,6406:1,6407:3,6408:4,6409:1,6410:2,33319:2,33320:2,35904:3,35906:4,36244:1,36248:3,36249:4};
  
  var __sizeOfGlTextureElementType={5120:1,5121:1,5122:2,5123:2,5124:4,5125:4,5126:4,5131:2,32819:2,32820:2,33635:2,33640:4,34042:4,35899:4,35902:4,36193:2};function emscriptenWebGLGetTexPixelData(type, format, width, height, pixels, internalFormat) {
      var sizePerPixel = __colorChannelsInGlTextureFormat[format] * __sizeOfGlTextureElementType[type];
      if (!sizePerPixel) {
        GL.recordError(0x0500); // GL_INVALID_ENUM
        return;
      }
      var bytes = __computeUnpackAlignedImageSize(width, height, sizePerPixel, GL.unpackAlignment);
      var end = pixels + bytes;
      switch(type) {
        case 0x1400 /* GL_BYTE */:
          return HEAP8.subarray(pixels, end);
        case 0x1401 /* GL_UNSIGNED_BYTE */:
          return HEAPU8.subarray(pixels, end);
        case 0x1402 /* GL_SHORT */:
          return HEAP16.subarray(pixels>>1, end>>1);
        case 0x1404 /* GL_INT */:
          return HEAP32.subarray(pixels>>2, end>>2);
        case 0x1406 /* GL_FLOAT */:
          return HEAPF32.subarray(pixels>>2, end>>2);
        case 0x1405 /* GL_UNSIGNED_INT */:
        case 0x84FA /* GL_UNSIGNED_INT_24_8_WEBGL/GL_UNSIGNED_INT_24_8 */:
        case 0x8C3E /* GL_UNSIGNED_INT_5_9_9_9_REV */:
        case 0x8368 /* GL_UNSIGNED_INT_2_10_10_10_REV */:
        case 0x8C3B /* GL_UNSIGNED_INT_10F_11F_11F_REV */:
          return HEAPU32.subarray(pixels>>2, end>>2);
        case 0x1403 /* GL_UNSIGNED_SHORT */:
        case 0x8363 /* GL_UNSIGNED_SHORT_5_6_5 */:
        case 0x8033 /* GL_UNSIGNED_SHORT_4_4_4_4 */:
        case 0x8034 /* GL_UNSIGNED_SHORT_5_5_5_1 */:
        case 0x8D61 /* GL_HALF_FLOAT_OES */:
        case 0x140B /* GL_HALF_FLOAT */:
          return HEAPU16.subarray(pixels>>1, end>>1);
        default:
          GL.recordError(0x0500); // GL_INVALID_ENUM
      }
    }
  
  function __heapObjectForWebGLType(type) {
      switch(type) {
        case 0x1400 /* GL_BYTE */:
          return HEAP8;
        case 0x1401 /* GL_UNSIGNED_BYTE */:
          return HEAPU8;
        case 0x1402 /* GL_SHORT */:
          return HEAP16;
        case 0x1403 /* GL_UNSIGNED_SHORT */:
        case 0x8363 /* GL_UNSIGNED_SHORT_5_6_5 */:
        case 0x8033 /* GL_UNSIGNED_SHORT_4_4_4_4 */:
        case 0x8034 /* GL_UNSIGNED_SHORT_5_5_5_1 */:
        case 0x8D61 /* GL_HALF_FLOAT_OES */:
        case 0x140B /* GL_HALF_FLOAT */:
          return HEAPU16;
        case 0x1404 /* GL_INT */:
          return HEAP32;
        case 0x1405 /* GL_UNSIGNED_INT */:
        case 0x84FA /* GL_UNSIGNED_INT_24_8_WEBGL/GL_UNSIGNED_INT_24_8 */:
        case 0x8C3E /* GL_UNSIGNED_INT_5_9_9_9_REV */:
        case 0x8368 /* GL_UNSIGNED_INT_2_10_10_10_REV */:
        case 0x8C3B /* GL_UNSIGNED_INT_10F_11F_11F_REV */:
        case 0x84FA /* GL_UNSIGNED_INT_24_8 */:
          return HEAPU32;
        case 0x1406 /* GL_FLOAT */:
          return HEAPF32;
      }
    }
  
  var __heapAccessShiftForWebGLType={5122:1,5123:1,5124:2,5125:2,5126:2,5131:1,32819:1,32820:1,33635:1,33640:2,34042:2,35899:2,35902:2,36193:1};function _emscripten_glReadPixels(x, y, width, height, format, type, pixels) {
      if (GL.currentContext.supportsWebGL2EntryPoints) { // WebGL 2 provides new garbage-free entry points to call to WebGL. Use those always when possible.
        if (GLctx.currentPixelPackBufferBinding) {
          GLctx.readPixels(x, y, width, height, format, type, pixels);
        } else {
          GLctx.readPixels(x, y, width, height, format, type, __heapObjectForWebGLType(type), pixels >> (__heapAccessShiftForWebGLType[type]|0));
        }
        return;
      }
      var pixelData = emscriptenWebGLGetTexPixelData(type, format, width, height, pixels, format);
      if (!pixelData) {
        GL.recordError(0x0500/*GL_INVALID_ENUM*/);
        return;
      }
      GLctx.readPixels(x, y, width, height, format, type, pixelData);
    }

  function _emscripten_glReleaseShaderCompiler() {
      // NOP (as allowed by GLES 2.0 spec)
    }

  function _emscripten_glRenderbufferStorage(x0, x1, x2, x3) { GLctx['renderbufferStorage'](x0, x1, x2, x3) }

  function _emscripten_glRenderbufferStorageMultisample(x0, x1, x2, x3, x4) { GLctx['renderbufferStorageMultisample'](x0, x1, x2, x3, x4) }

  function _emscripten_glResumeTransformFeedback() { GLctx['resumeTransformFeedback']() }

  function _emscripten_glSampleCoverage(value, invert) {
      GLctx.sampleCoverage(value, !!invert);
    }

  function _emscripten_glSamplerParameterf(sampler, pname, param) {
      GLctx['samplerParameterf'](GL.samplers[sampler], pname, param);
    }

  function _emscripten_glSamplerParameterfv(sampler, pname, params) {
      var param = HEAPF32[((params)>>2)];
      GLctx['samplerParameterf'](GL.samplers[sampler], pname, param);
    }

  function _emscripten_glSamplerParameteri(sampler, pname, param) {
      GLctx['samplerParameteri'](GL.samplers[sampler], pname, param);
    }

  function _emscripten_glSamplerParameteriv(sampler, pname, params) {
      var param = HEAP32[((params)>>2)];
      GLctx['samplerParameteri'](GL.samplers[sampler], pname, param);
    }

  function _emscripten_glScissor(x0, x1, x2, x3) { GLctx['scissor'](x0, x1, x2, x3) }

  function _emscripten_glShaderBinary() {
      GL.recordError(0x0500/*GL_INVALID_ENUM*/);
    }

  function _emscripten_glShaderSource(shader, count, string, length) {
      var source = GL.getSource(shader, count, string, length);
  
  
      GLctx.shaderSource(GL.shaders[shader], source);
    }

  function _emscripten_glStencilFunc(x0, x1, x2) { GLctx['stencilFunc'](x0, x1, x2) }

  function _emscripten_glStencilFuncSeparate(x0, x1, x2, x3) { GLctx['stencilFuncSeparate'](x0, x1, x2, x3) }

  function _emscripten_glStencilMask(x0) { GLctx['stencilMask'](x0) }

  function _emscripten_glStencilMaskSeparate(x0, x1) { GLctx['stencilMaskSeparate'](x0, x1) }

  function _emscripten_glStencilOp(x0, x1, x2) { GLctx['stencilOp'](x0, x1, x2) }

  function _emscripten_glStencilOpSeparate(x0, x1, x2, x3) { GLctx['stencilOpSeparate'](x0, x1, x2, x3) }

  function _emscripten_glTexImage2D(target, level, internalFormat, width, height, border, format, type, pixels) {
      if (GL.currentContext.supportsWebGL2EntryPoints) {
        // WebGL 2 provides new garbage-free entry points to call to WebGL. Use those always when possible.
        if (GLctx.currentPixelUnpackBufferBinding) {
          GLctx.texImage2D(target, level, internalFormat, width, height, border, format, type, pixels);
        } else if (pixels != 0) {
          GLctx.texImage2D(target, level, internalFormat, width, height, border, format, type, __heapObjectForWebGLType(type), pixels >> (__heapAccessShiftForWebGLType[type]|0));
        } else {
          GLctx.texImage2D(target, level, internalFormat, width, height, border, format, type, null);
        }
        return;
      }
      GLctx.texImage2D(target, level, internalFormat, width, height, border, format, type, pixels ? emscriptenWebGLGetTexPixelData(type, format, width, height, pixels, internalFormat) : null);
    }

  function _emscripten_glTexImage3D(target, level, internalFormat, width, height, depth, border, format, type, pixels) {
      if (GLctx.currentPixelUnpackBufferBinding) {
        GLctx['texImage3D'](target, level, internalFormat, width, height, depth, border, format, type, pixels);
      } else if (pixels != 0) {
        GLctx['texImage3D'](target, level, internalFormat, width, height, depth, border, format, type, __heapObjectForWebGLType(type), pixels >> (__heapAccessShiftForWebGLType[type]|0));
      } else {
        GLctx['texImage3D'](target, level, internalFormat, width, height, depth, border, format, type, null);
      }
    }

  function _emscripten_glTexParameterf(x0, x1, x2) { GLctx['texParameterf'](x0, x1, x2) }

  function _emscripten_glTexParameterfv(target, pname, params) {
      var param = HEAPF32[((params)>>2)];
      GLctx.texParameterf(target, pname, param);
    }

  function _emscripten_glTexParameteri(x0, x1, x2) { GLctx['texParameteri'](x0, x1, x2) }

  function _emscripten_glTexParameteriv(target, pname, params) {
      var param = HEAP32[((params)>>2)];
      GLctx.texParameteri(target, pname, param);
    }

  function _emscripten_glTexStorage2D(x0, x1, x2, x3, x4) { GLctx['texStorage2D'](x0, x1, x2, x3, x4) }

  function _emscripten_glTexStorage3D(x0, x1, x2, x3, x4, x5) { GLctx['texStorage3D'](x0, x1, x2, x3, x4, x5) }

  function _emscripten_glTexSubImage2D(target, level, xoffset, yoffset, width, height, format, type, pixels) {
      if (GL.currentContext.supportsWebGL2EntryPoints) {
        // WebGL 2 provides new garbage-free entry points to call to WebGL. Use those always when possible.
        if (GLctx.currentPixelUnpackBufferBinding) {
          GLctx.texSubImage2D(target, level, xoffset, yoffset, width, height, format, type, pixels);
        } else if (pixels != 0) {
          GLctx.texSubImage2D(target, level, xoffset, yoffset, width, height, format, type, __heapObjectForWebGLType(type), pixels >> (__heapAccessShiftForWebGLType[type]|0));
        } else {
          GLctx.texSubImage2D(target, level, xoffset, yoffset, width, height, format, type, null);
        }
        return;
      }
      var pixelData = null;
      if (pixels) pixelData = emscriptenWebGLGetTexPixelData(type, format, width, height, pixels, 0);
      GLctx.texSubImage2D(target, level, xoffset, yoffset, width, height, format, type, pixelData);
    }

  function _emscripten_glTexSubImage3D(target, level, xoffset, yoffset, zoffset, width, height, depth, format, type, pixels) {
      if (GLctx.currentPixelUnpackBufferBinding) {
        GLctx['texSubImage3D'](target, level, xoffset, yoffset, zoffset, width, height, depth, format, type, pixels);
      } else if (pixels != 0) {
        GLctx['texSubImage3D'](target, level, xoffset, yoffset, zoffset, width, height, depth, format, type, __heapObjectForWebGLType(type), pixels >> (__heapAccessShiftForWebGLType[type]|0));
      } else {
        GLctx['texSubImage3D'](target, level, xoffset, yoffset, zoffset, width, height, depth, format, type, null);
      }
    }

  function _emscripten_glTransformFeedbackVaryings(program, count, varyings, bufferMode) {
      program = GL.programs[program];
      var vars = [];
      for (var i = 0; i < count; i++)
        vars.push(UTF8ToString(HEAP32[(((varyings)+(i*4))>>2)]));
  
      GLctx['transformFeedbackVaryings'](program, vars, bufferMode);
    }

  function _emscripten_glUniform1f(location, v0) {
      GLctx.uniform1f(GL.uniforms[location], v0);
    }

  function _emscripten_glUniform1fv(location, count, value) {
  
      if (GL.currentContext.supportsWebGL2EntryPoints) { // WebGL 2 provides new garbage-free entry points to call to WebGL. Use those always when possible.
        GLctx.uniform1fv(GL.uniforms[location], HEAPF32, value>>2, count);
        return;
      }
  
      if (count <= GL.MINI_TEMP_BUFFER_SIZE) {
        // avoid allocation when uploading few enough uniforms
        var view = GL.miniTempBufferViews[count-1];
        for (var i = 0; i < count; ++i) {
          view[i] = HEAPF32[(((value)+(4*i))>>2)];
        }
      } else
      {
        var view = HEAPF32.subarray((value)>>2,(value+count*4)>>2);
      }
      GLctx.uniform1fv(GL.uniforms[location], view);
    }

  function _emscripten_glUniform1i(location, v0) {
      GLctx.uniform1i(GL.uniforms[location], v0);
    }

  function _emscripten_glUniform1iv(location, count, value) {
  
      if (GL.currentContext.supportsWebGL2EntryPoints) { // WebGL 2 provides new garbage-free entry points to call to WebGL. Use those always when possible.
        GLctx.uniform1iv(GL.uniforms[location], HEAP32, value>>2, count);
        return;
      }
  
      GLctx.uniform1iv(GL.uniforms[location], HEAP32.subarray((value)>>2,(value+count*4)>>2));
    }

  function _emscripten_glUniform1ui(location, v0) {
      GLctx.uniform1ui(GL.uniforms[location], v0);
    }

  function _emscripten_glUniform1uiv(location, count, value) {
      if (GL.currentContext.supportsWebGL2EntryPoints) { // WebGL 2 provides new garbage-free entry points to call to WebGL. Use those always when possible.
        GLctx.uniform1uiv(GL.uniforms[location], HEAPU32, value>>2, count);
      } else {
        GLctx.uniform1uiv(GL.uniforms[location], HEAPU32.subarray((value)>>2,(value+count*4)>>2));
      }
    }

  function _emscripten_glUniform2f(location, v0, v1) {
      GLctx.uniform2f(GL.uniforms[location], v0, v1);
    }

  function _emscripten_glUniform2fv(location, count, value) {
  
      if (GL.currentContext.supportsWebGL2EntryPoints) { // WebGL 2 provides new garbage-free entry points to call to WebGL. Use those always when possible.
        GLctx.uniform2fv(GL.uniforms[location], HEAPF32, value>>2, count*2);
        return;
      }
  
      if (2*count <= GL.MINI_TEMP_BUFFER_SIZE) {
        // avoid allocation when uploading few enough uniforms
        var view = GL.miniTempBufferViews[2*count-1];
        for (var i = 0; i < 2*count; i += 2) {
          view[i] = HEAPF32[(((value)+(4*i))>>2)];
          view[i+1] = HEAPF32[(((value)+(4*i+4))>>2)];
        }
      } else
      {
        var view = HEAPF32.subarray((value)>>2,(value+count*8)>>2);
      }
      GLctx.uniform2fv(GL.uniforms[location], view);
    }

  function _emscripten_glUniform2i(location, v0, v1) {
      GLctx.uniform2i(GL.uniforms[location], v0, v1);
    }

  function _emscripten_glUniform2iv(location, count, value) {
  
      if (GL.currentContext.supportsWebGL2EntryPoints) { // WebGL 2 provides new garbage-free entry points to call to WebGL. Use those always when possible.
        GLctx.uniform2iv(GL.uniforms[location], HEAP32, value>>2, count*2);
        return;
      }
  
      GLctx.uniform2iv(GL.uniforms[location], HEAP32.subarray((value)>>2,(value+count*8)>>2));
    }

  function _emscripten_glUniform2ui(location, v0, v1) {
      GLctx.uniform2ui(GL.uniforms[location], v0, v1);
    }

  function _emscripten_glUniform2uiv(location, count, value) {
      if (GL.currentContext.supportsWebGL2EntryPoints) { // WebGL 2 provides new garbage-free entry points to call to WebGL. Use those always when possible.
        GLctx.uniform2uiv(GL.uniforms[location], HEAPU32, value>>2, count*2);
      } else {
        GLctx.uniform2uiv(GL.uniforms[location], HEAPU32.subarray((value)>>2,(value+count*8)>>2));
      }
    }

  function _emscripten_glUniform3f(location, v0, v1, v2) {
      GLctx.uniform3f(GL.uniforms[location], v0, v1, v2);
    }

  function _emscripten_glUniform3fv(location, count, value) {
  
      if (GL.currentContext.supportsWebGL2EntryPoints) { // WebGL 2 provides new garbage-free entry points to call to WebGL. Use those always when possible.
        GLctx.uniform3fv(GL.uniforms[location], HEAPF32, value>>2, count*3);
        return;
      }
  
      if (3*count <= GL.MINI_TEMP_BUFFER_SIZE) {
        // avoid allocation when uploading few enough uniforms
        var view = GL.miniTempBufferViews[3*count-1];
        for (var i = 0; i < 3*count; i += 3) {
          view[i] = HEAPF32[(((value)+(4*i))>>2)];
          view[i+1] = HEAPF32[(((value)+(4*i+4))>>2)];
          view[i+2] = HEAPF32[(((value)+(4*i+8))>>2)];
        }
      } else
      {
        var view = HEAPF32.subarray((value)>>2,(value+count*12)>>2);
      }
      GLctx.uniform3fv(GL.uniforms[location], view);
    }

  function _emscripten_glUniform3i(location, v0, v1, v2) {
      GLctx.uniform3i(GL.uniforms[location], v0, v1, v2);
    }

  function _emscripten_glUniform3iv(location, count, value) {
  
      if (GL.currentContext.supportsWebGL2EntryPoints) { // WebGL 2 provides new garbage-free entry points to call to WebGL. Use those always when possible.
        GLctx.uniform3iv(GL.uniforms[location], HEAP32, value>>2, count*3);
        return;
      }
  
      GLctx.uniform3iv(GL.uniforms[location], HEAP32.subarray((value)>>2,(value+count*12)>>2));
    }

  function _emscripten_glUniform3ui(location, v0, v1, v2) {
      GLctx.uniform3ui(GL.uniforms[location], v0, v1, v2);
    }

  function _emscripten_glUniform3uiv(location, count, value) {
      if (GL.currentContext.supportsWebGL2EntryPoints) { // WebGL 2 provides new garbage-free entry points to call to WebGL. Use those always when possible.
        GLctx.uniform3uiv(GL.uniforms[location], HEAPU32, value>>2, count*3);
      } else {
        GLctx.uniform3uiv(GL.uniforms[location], HEAPU32.subarray((value)>>2,(value+count*12)>>2));
      }
    }

  function _emscripten_glUniform4f(location, v0, v1, v2, v3) {
      GLctx.uniform4f(GL.uniforms[location], v0, v1, v2, v3);
    }

  function _emscripten_glUniform4fv(location, count, value) {
  
      if (GL.currentContext.supportsWebGL2EntryPoints) { // WebGL 2 provides new garbage-free entry points to call to WebGL. Use those always when possible.
        GLctx.uniform4fv(GL.uniforms[location], HEAPF32, value>>2, count*4);
        return;
      }
  
      if (4*count <= GL.MINI_TEMP_BUFFER_SIZE) {
        // avoid allocation when uploading few enough uniforms
        var view = GL.miniTempBufferViews[4*count-1];
        for (var i = 0; i < 4*count; i += 4) {
          view[i] = HEAPF32[(((value)+(4*i))>>2)];
          view[i+1] = HEAPF32[(((value)+(4*i+4))>>2)];
          view[i+2] = HEAPF32[(((value)+(4*i+8))>>2)];
          view[i+3] = HEAPF32[(((value)+(4*i+12))>>2)];
        }
      } else
      {
        var view = HEAPF32.subarray((value)>>2,(value+count*16)>>2);
      }
      GLctx.uniform4fv(GL.uniforms[location], view);
    }

  function _emscripten_glUniform4i(location, v0, v1, v2, v3) {
      GLctx.uniform4i(GL.uniforms[location], v0, v1, v2, v3);
    }

  function _emscripten_glUniform4iv(location, count, value) {
  
      if (GL.currentContext.supportsWebGL2EntryPoints) { // WebGL 2 provides new garbage-free entry points to call to WebGL. Use those always when possible.
        GLctx.uniform4iv(GL.uniforms[location], HEAP32, value>>2, count*4);
        return;
      }
  
      GLctx.uniform4iv(GL.uniforms[location], HEAP32.subarray((value)>>2,(value+count*16)>>2));
    }

  function _emscripten_glUniform4ui(location, v0, v1, v2, v3) {
      GLctx.uniform4ui(GL.uniforms[location], v0, v1, v2, v3);
    }

  function _emscripten_glUniform4uiv(location, count, value) {
      if (GL.currentContext.supportsWebGL2EntryPoints) { // WebGL 2 provides new garbage-free entry points to call to WebGL. Use those always when possible.
        GLctx.uniform4uiv(GL.uniforms[location], HEAPU32, value>>2, count*4);
      } else {
        GLctx.uniform4uiv(GL.uniforms[location], HEAPU32.subarray((value)>>2,(value+count*16)>>2));
      }
    }

  function _emscripten_glUniformBlockBinding(program, uniformBlockIndex, uniformBlockBinding) {
      program = GL.programs[program];
  
      GLctx['uniformBlockBinding'](program, uniformBlockIndex, uniformBlockBinding);
    }

  function _emscripten_glUniformMatrix2fv(location, count, transpose, value) {
  
      if (GL.currentContext.supportsWebGL2EntryPoints) { // WebGL 2 provides new garbage-free entry points to call to WebGL. Use those always when possible.
        GLctx.uniformMatrix2fv(GL.uniforms[location], !!transpose, HEAPF32, value>>2, count*4);
        return;
      }
  
      if (4*count <= GL.MINI_TEMP_BUFFER_SIZE) {
        // avoid allocation when uploading few enough uniforms
        var view = GL.miniTempBufferViews[4*count-1];
        for (var i = 0; i < 4*count; i += 4) {
          view[i] = HEAPF32[(((value)+(4*i))>>2)];
          view[i+1] = HEAPF32[(((value)+(4*i+4))>>2)];
          view[i+2] = HEAPF32[(((value)+(4*i+8))>>2)];
          view[i+3] = HEAPF32[(((value)+(4*i+12))>>2)];
        }
      } else
      {
        var view = HEAPF32.subarray((value)>>2,(value+count*16)>>2);
      }
      GLctx.uniformMatrix2fv(GL.uniforms[location], !!transpose, view);
    }

  function _emscripten_glUniformMatrix2x3fv(location, count, transpose, value) {
      if (GL.currentContext.supportsWebGL2EntryPoints) { // WebGL 2 provides new garbage-free entry points to call to WebGL. Use those always when possible.
        GLctx.uniformMatrix2x3fv(GL.uniforms[location], !!transpose, HEAPF32, value>>2, count*6);
      } else {
        GLctx.uniformMatrix2x3fv(GL.uniforms[location], !!transpose, HEAPF32.subarray((value)>>2,(value+count*24)>>2));
      }
    }

  function _emscripten_glUniformMatrix2x4fv(location, count, transpose, value) {
      if (GL.currentContext.supportsWebGL2EntryPoints) { // WebGL 2 provides new garbage-free entry points to call to WebGL. Use those always when possible.
        GLctx.uniformMatrix2x4fv(GL.uniforms[location], !!transpose, HEAPF32, value>>2, count*8);
      } else {
        GLctx.uniformMatrix2x4fv(GL.uniforms[location], !!transpose, HEAPF32.subarray((value)>>2,(value+count*32)>>2));
      }
    }

  function _emscripten_glUniformMatrix3fv(location, count, transpose, value) {
  
      if (GL.currentContext.supportsWebGL2EntryPoints) { // WebGL 2 provides new garbage-free entry points to call to WebGL. Use those always when possible.
        GLctx.uniformMatrix3fv(GL.uniforms[location], !!transpose, HEAPF32, value>>2, count*9);
        return;
      }
  
      if (9*count <= GL.MINI_TEMP_BUFFER_SIZE) {
        // avoid allocation when uploading few enough uniforms
        var view = GL.miniTempBufferViews[9*count-1];
        for (var i = 0; i < 9*count; i += 9) {
          view[i] = HEAPF32[(((value)+(4*i))>>2)];
          view[i+1] = HEAPF32[(((value)+(4*i+4))>>2)];
          view[i+2] = HEAPF32[(((value)+(4*i+8))>>2)];
          view[i+3] = HEAPF32[(((value)+(4*i+12))>>2)];
          view[i+4] = HEAPF32[(((value)+(4*i+16))>>2)];
          view[i+5] = HEAPF32[(((value)+(4*i+20))>>2)];
          view[i+6] = HEAPF32[(((value)+(4*i+24))>>2)];
          view[i+7] = HEAPF32[(((value)+(4*i+28))>>2)];
          view[i+8] = HEAPF32[(((value)+(4*i+32))>>2)];
        }
      } else
      {
        var view = HEAPF32.subarray((value)>>2,(value+count*36)>>2);
      }
      GLctx.uniformMatrix3fv(GL.uniforms[location], !!transpose, view);
    }

  function _emscripten_glUniformMatrix3x2fv(location, count, transpose, value) {
      if (GL.currentContext.supportsWebGL2EntryPoints) { // WebGL 2 provides new garbage-free entry points to call to WebGL. Use those always when possible.
        GLctx.uniformMatrix3x2fv(GL.uniforms[location], !!transpose, HEAPF32, value>>2, count*6);
      } else {
        GLctx.uniformMatrix3x2fv(GL.uniforms[location], !!transpose, HEAPF32.subarray((value)>>2,(value+count*24)>>2));
      }
    }

  function _emscripten_glUniformMatrix3x4fv(location, count, transpose, value) {
      if (GL.currentContext.supportsWebGL2EntryPoints) { // WebGL 2 provides new garbage-free entry points to call to WebGL. Use those always when possible.
        GLctx.uniformMatrix3x4fv(GL.uniforms[location], !!transpose, HEAPF32, value>>2, count*12);
      } else {
        GLctx.uniformMatrix3x4fv(GL.uniforms[location], !!transpose, HEAPF32.subarray((value)>>2,(value+count*48)>>2));
      }
    }

  function _emscripten_glUniformMatrix4fv(location, count, transpose, value) {
  
      if (GL.currentContext.supportsWebGL2EntryPoints) { // WebGL 2 provides new garbage-free entry points to call to WebGL. Use those always when possible.
        GLctx.uniformMatrix4fv(GL.uniforms[location], !!transpose, HEAPF32, value>>2, count*16);
        return;
      }
  
      if (16*count <= GL.MINI_TEMP_BUFFER_SIZE) {
        // avoid allocation when uploading few enough uniforms
        var view = GL.miniTempBufferViews[16*count-1];
        for (var i = 0; i < 16*count; i += 16) {
          view[i] = HEAPF32[(((value)+(4*i))>>2)];
          view[i+1] = HEAPF32[(((value)+(4*i+4))>>2)];
          view[i+2] = HEAPF32[(((value)+(4*i+8))>>2)];
          view[i+3] = HEAPF32[(((value)+(4*i+12))>>2)];
          view[i+4] = HEAPF32[(((value)+(4*i+16))>>2)];
          view[i+5] = HEAPF32[(((value)+(4*i+20))>>2)];
          view[i+6] = HEAPF32[(((value)+(4*i+24))>>2)];
          view[i+7] = HEAPF32[(((value)+(4*i+28))>>2)];
          view[i+8] = HEAPF32[(((value)+(4*i+32))>>2)];
          view[i+9] = HEAPF32[(((value)+(4*i+36))>>2)];
          view[i+10] = HEAPF32[(((value)+(4*i+40))>>2)];
          view[i+11] = HEAPF32[(((value)+(4*i+44))>>2)];
          view[i+12] = HEAPF32[(((value)+(4*i+48))>>2)];
          view[i+13] = HEAPF32[(((value)+(4*i+52))>>2)];
          view[i+14] = HEAPF32[(((value)+(4*i+56))>>2)];
          view[i+15] = HEAPF32[(((value)+(4*i+60))>>2)];
        }
      } else
      {
        var view = HEAPF32.subarray((value)>>2,(value+count*64)>>2);
      }
      GLctx.uniformMatrix4fv(GL.uniforms[location], !!transpose, view);
    }

  function _emscripten_glUniformMatrix4x2fv(location, count, transpose, value) {
      if (GL.currentContext.supportsWebGL2EntryPoints) { // WebGL 2 provides new garbage-free entry points to call to WebGL. Use those always when possible.
        GLctx.uniformMatrix4x2fv(GL.uniforms[location], !!transpose, HEAPF32, value>>2, count*8);
      } else {
        GLctx.uniformMatrix4x2fv(GL.uniforms[location], !!transpose, HEAPF32.subarray((value)>>2,(value+count*32)>>2));
      }
    }

  function _emscripten_glUniformMatrix4x3fv(location, count, transpose, value) {
      if (GL.currentContext.supportsWebGL2EntryPoints) { // WebGL 2 provides new garbage-free entry points to call to WebGL. Use those always when possible.
        GLctx.uniformMatrix4x3fv(GL.uniforms[location], !!transpose, HEAPF32, value>>2, count*12);
      } else {
        GLctx.uniformMatrix4x3fv(GL.uniforms[location], !!transpose, HEAPF32.subarray((value)>>2,(value+count*48)>>2));
      }
    }

  function _emscripten_glUnmapBuffer(
  ) {
  err('missing function: emscripten_glUnmapBuffer'); abort(-1);
  }

  function _emscripten_glUseProgram(program) {
      GLctx.useProgram(GL.programs[program]);
    }

  function _emscripten_glValidateProgram(program) {
      GLctx.validateProgram(GL.programs[program]);
    }

  function _emscripten_glVertexAttrib1f(x0, x1) { GLctx['vertexAttrib1f'](x0, x1) }

  function _emscripten_glVertexAttrib1fv(index, v) {
  
      GLctx.vertexAttrib1f(index, HEAPF32[v>>2]);
    }

  function _emscripten_glVertexAttrib2f(x0, x1, x2) { GLctx['vertexAttrib2f'](x0, x1, x2) }

  function _emscripten_glVertexAttrib2fv(index, v) {
  
      GLctx.vertexAttrib2f(index, HEAPF32[v>>2], HEAPF32[v+4>>2]);
    }

  function _emscripten_glVertexAttrib3f(x0, x1, x2, x3) { GLctx['vertexAttrib3f'](x0, x1, x2, x3) }

  function _emscripten_glVertexAttrib3fv(index, v) {
  
      GLctx.vertexAttrib3f(index, HEAPF32[v>>2], HEAPF32[v+4>>2], HEAPF32[v+8>>2]);
    }

  function _emscripten_glVertexAttrib4f(x0, x1, x2, x3, x4) { GLctx['vertexAttrib4f'](x0, x1, x2, x3, x4) }

  function _emscripten_glVertexAttrib4fv(index, v) {
  
      GLctx.vertexAttrib4f(index, HEAPF32[v>>2], HEAPF32[v+4>>2], HEAPF32[v+8>>2], HEAPF32[v+12>>2]);
    }

  function _emscripten_glVertexAttribDivisor(index, divisor) {
      GLctx['vertexAttribDivisor'](index, divisor);
    }

  function _emscripten_glVertexAttribDivisorANGLE(index, divisor) {
      GLctx['vertexAttribDivisor'](index, divisor);
    }

  function _emscripten_glVertexAttribDivisorARB(index, divisor) {
      GLctx['vertexAttribDivisor'](index, divisor);
    }

  function _emscripten_glVertexAttribDivisorEXT(index, divisor) {
      GLctx['vertexAttribDivisor'](index, divisor);
    }

  function _emscripten_glVertexAttribDivisorNV(index, divisor) {
      GLctx['vertexAttribDivisor'](index, divisor);
    }

  function _emscripten_glVertexAttribI4i(x0, x1, x2, x3, x4) { GLctx['vertexAttribI4i'](x0, x1, x2, x3, x4) }

  function _emscripten_glVertexAttribI4iv(index, v) {
      GLctx.vertexAttribI4i(index, HEAP32[v>>2], HEAP32[v+4>>2], HEAP32[v+8>>2], HEAP32[v+12>>2]);
    }

  function _emscripten_glVertexAttribI4ui(x0, x1, x2, x3, x4) { GLctx['vertexAttribI4ui'](x0, x1, x2, x3, x4) }

  function _emscripten_glVertexAttribI4uiv(index, v) {
      GLctx.vertexAttribI4ui(index, HEAPU32[v>>2], HEAPU32[v+4>>2], HEAPU32[v+8>>2], HEAPU32[v+12>>2]);
    }

  function _emscripten_glVertexAttribIPointer(index, size, type, stride, ptr) {
      GLctx['vertexAttribIPointer'](index, size, type, stride, ptr);
    }

  function _emscripten_glVertexAttribPointer(index, size, type, normalized, stride, ptr) {
      GLctx.vertexAttribPointer(index, size, type, !!normalized, stride, ptr);
    }

  function _emscripten_glViewport(x0, x1, x2, x3) { GLctx['viewport'](x0, x1, x2, x3) }

  function _emscripten_glWaitSync(sync, flags, timeoutLo, timeoutHi) {
      // See WebGL2 vs GLES3 difference on GL_TIMEOUT_IGNORED above (https://www.khronos.org/registry/webgl/specs/latest/2.0/#5.15)
      timeoutLo = timeoutLo >>> 0;
      timeoutHi = timeoutHi >>> 0;
      var timeout = (timeoutLo == 0xFFFFFFFF && timeoutHi == 0xFFFFFFFF) ? -1 : makeBigInt(timeoutLo, timeoutHi, true);
      GLctx.waitSync(GL.syncs[sync], flags, timeout);
    }

   

  
  
  function __reallyNegative(x) {
      return x < 0 || (x === 0 && (1/x) === -Infinity);
    }function __formatString(format, varargs) {
      assert((varargs & 3) === 0);
      var textIndex = format;
      var argIndex = varargs;
      // This must be called before reading a double or i64 vararg. It will bump the pointer properly.
      // It also does an assert on i32 values, so it's nice to call it before all varargs calls.
      function prepVararg(ptr, type) {
        if (type === 'double' || type === 'i64') {
          // move so the load is aligned
          if (ptr & 7) {
            assert((ptr & 7) === 4);
            ptr += 4;
          }
        } else {
          assert((ptr & 3) === 0);
        }
        return ptr;
      }
      function getNextArg(type) {
        // NOTE: Explicitly ignoring type safety. Otherwise this fails:
        //       int x = 4; printf("%c\n", (char)x);
        var ret;
        argIndex = prepVararg(argIndex, type);
        if (type === 'double') {
          ret = HEAPF64[((argIndex)>>3)];
          argIndex += 8;
        } else if (type == 'i64') {
          ret = [HEAP32[((argIndex)>>2)],
                 HEAP32[(((argIndex)+(4))>>2)]];
          argIndex += 8;
        } else {
          assert((argIndex & 3) === 0);
          type = 'i32'; // varargs are always i32, i64, or double
          ret = HEAP32[((argIndex)>>2)];
          argIndex += 4;
        }
        return ret;
      }
  
      var ret = [];
      var curr, next, currArg;
      while(1) {
        var startTextIndex = textIndex;
        curr = HEAP8[((textIndex)>>0)];
        if (curr === 0) break;
        next = HEAP8[((textIndex+1)>>0)];
        if (curr == 37) {
          // Handle flags.
          var flagAlwaysSigned = false;
          var flagLeftAlign = false;
          var flagAlternative = false;
          var flagZeroPad = false;
          var flagPadSign = false;
          flagsLoop: while (1) {
            switch (next) {
              case 43:
                flagAlwaysSigned = true;
                break;
              case 45:
                flagLeftAlign = true;
                break;
              case 35:
                flagAlternative = true;
                break;
              case 48:
                if (flagZeroPad) {
                  break flagsLoop;
                } else {
                  flagZeroPad = true;
                  break;
                }
              case 32:
                flagPadSign = true;
                break;
              default:
                break flagsLoop;
            }
            textIndex++;
            next = HEAP8[((textIndex+1)>>0)];
          }
  
          // Handle width.
          var width = 0;
          if (next == 42) {
            width = getNextArg('i32');
            textIndex++;
            next = HEAP8[((textIndex+1)>>0)];
          } else {
            while (next >= 48 && next <= 57) {
              width = width * 10 + (next - 48);
              textIndex++;
              next = HEAP8[((textIndex+1)>>0)];
            }
          }
  
          // Handle precision.
          var precisionSet = false, precision = -1;
          if (next == 46) {
            precision = 0;
            precisionSet = true;
            textIndex++;
            next = HEAP8[((textIndex+1)>>0)];
            if (next == 42) {
              precision = getNextArg('i32');
              textIndex++;
            } else {
              while(1) {
                var precisionChr = HEAP8[((textIndex+1)>>0)];
                if (precisionChr < 48 ||
                    precisionChr > 57) break;
                precision = precision * 10 + (precisionChr - 48);
                textIndex++;
              }
            }
            next = HEAP8[((textIndex+1)>>0)];
          }
          if (precision < 0) {
            precision = 6; // Standard default.
            precisionSet = false;
          }
  
          // Handle integer sizes. WARNING: These assume a 32-bit architecture!
          var argSize;
          switch (String.fromCharCode(next)) {
            case 'h':
              var nextNext = HEAP8[((textIndex+2)>>0)];
              if (nextNext == 104) {
                textIndex++;
                argSize = 1; // char (actually i32 in varargs)
              } else {
                argSize = 2; // short (actually i32 in varargs)
              }
              break;
            case 'l':
              var nextNext = HEAP8[((textIndex+2)>>0)];
              if (nextNext == 108) {
                textIndex++;
                argSize = 8; // long long
              } else {
                argSize = 4; // long
              }
              break;
            case 'L': // long long
            case 'q': // int64_t
            case 'j': // intmax_t
              argSize = 8;
              break;
            case 'z': // size_t
            case 't': // ptrdiff_t
            case 'I': // signed ptrdiff_t or unsigned size_t
              argSize = 4;
              break;
            default:
              argSize = null;
          }
          if (argSize) textIndex++;
          next = HEAP8[((textIndex+1)>>0)];
  
          // Handle type specifier.
          switch (String.fromCharCode(next)) {
            case 'd': case 'i': case 'u': case 'o': case 'x': case 'X': case 'p': {
              // Integer.
              var signed = next == 100 || next == 105;
              argSize = argSize || 4;
              currArg = getNextArg('i' + (argSize * 8));
              var argText;
              // Flatten i64-1 [low, high] into a (slightly rounded) double
              if (argSize == 8) {
                currArg = makeBigInt(currArg[0], currArg[1], next == 117);
              }
              // Truncate to requested size.
              if (argSize <= 4) {
                var limit = Math.pow(256, argSize) - 1;
                currArg = (signed ? reSign : unSign)(currArg & limit, argSize * 8);
              }
              // Format the number.
              var currAbsArg = Math.abs(currArg);
              var prefix = '';
              if (next == 100 || next == 105) {
                argText = reSign(currArg, 8 * argSize, 1).toString(10);
              } else if (next == 117) {
                argText = unSign(currArg, 8 * argSize, 1).toString(10);
                currArg = Math.abs(currArg);
              } else if (next == 111) {
                argText = (flagAlternative ? '0' : '') + currAbsArg.toString(8);
              } else if (next == 120 || next == 88) {
                prefix = (flagAlternative && currArg != 0) ? '0x' : '';
                if (currArg < 0) {
                  // Represent negative numbers in hex as 2's complement.
                  currArg = -currArg;
                  argText = (currAbsArg - 1).toString(16);
                  var buffer = [];
                  for (var i = 0; i < argText.length; i++) {
                    buffer.push((0xF - parseInt(argText[i], 16)).toString(16));
                  }
                  argText = buffer.join('');
                  while (argText.length < argSize * 2) argText = 'f' + argText;
                } else {
                  argText = currAbsArg.toString(16);
                }
                if (next == 88) {
                  prefix = prefix.toUpperCase();
                  argText = argText.toUpperCase();
                }
              } else if (next == 112) {
                if (currAbsArg === 0) {
                  argText = '(nil)';
                } else {
                  prefix = '0x';
                  argText = currAbsArg.toString(16);
                }
              }
              if (precisionSet) {
                while (argText.length < precision) {
                  argText = '0' + argText;
                }
              }
  
              // Add sign if needed
              if (currArg >= 0) {
                if (flagAlwaysSigned) {
                  prefix = '+' + prefix;
                } else if (flagPadSign) {
                  prefix = ' ' + prefix;
                }
              }
  
              // Move sign to prefix so we zero-pad after the sign
              if (argText.charAt(0) == '-') {
                prefix = '-' + prefix;
                argText = argText.substr(1);
              }
  
              // Add padding.
              while (prefix.length + argText.length < width) {
                if (flagLeftAlign) {
                  argText += ' ';
                } else {
                  if (flagZeroPad) {
                    argText = '0' + argText;
                  } else {
                    prefix = ' ' + prefix;
                  }
                }
              }
  
              // Insert the result into the buffer.
              argText = prefix + argText;
              argText.split('').forEach(function(chr) {
                ret.push(chr.charCodeAt(0));
              });
              break;
            }
            case 'f': case 'F': case 'e': case 'E': case 'g': case 'G': {
              // Float.
              currArg = getNextArg('double');
              var argText;
              if (isNaN(currArg)) {
                argText = 'nan';
                flagZeroPad = false;
              } else if (!isFinite(currArg)) {
                argText = (currArg < 0 ? '-' : '') + 'inf';
                flagZeroPad = false;
              } else {
                var isGeneral = false;
                var effectivePrecision = Math.min(precision, 20);
  
                // Convert g/G to f/F or e/E, as per:
                // http://pubs.opengroup.org/onlinepubs/9699919799/functions/printf.html
                if (next == 103 || next == 71) {
                  isGeneral = true;
                  precision = precision || 1;
                  var exponent = parseInt(currArg.toExponential(effectivePrecision).split('e')[1], 10);
                  if (precision > exponent && exponent >= -4) {
                    next = ((next == 103) ? 'f' : 'F').charCodeAt(0);
                    precision -= exponent + 1;
                  } else {
                    next = ((next == 103) ? 'e' : 'E').charCodeAt(0);
                    precision--;
                  }
                  effectivePrecision = Math.min(precision, 20);
                }
  
                if (next == 101 || next == 69) {
                  argText = currArg.toExponential(effectivePrecision);
                  // Make sure the exponent has at least 2 digits.
                  if (/[eE][-+]\d$/.test(argText)) {
                    argText = argText.slice(0, -1) + '0' + argText.slice(-1);
                  }
                } else if (next == 102 || next == 70) {
                  argText = currArg.toFixed(effectivePrecision);
                  if (currArg === 0 && __reallyNegative(currArg)) {
                    argText = '-' + argText;
                  }
                }
  
                var parts = argText.split('e');
                if (isGeneral && !flagAlternative) {
                  // Discard trailing zeros and periods.
                  while (parts[0].length > 1 && parts[0].indexOf('.') != -1 &&
                         (parts[0].slice(-1) == '0' || parts[0].slice(-1) == '.')) {
                    parts[0] = parts[0].slice(0, -1);
                  }
                } else {
                  // Make sure we have a period in alternative mode.
                  if (flagAlternative && argText.indexOf('.') == -1) parts[0] += '.';
                  // Zero pad until required precision.
                  while (precision > effectivePrecision++) parts[0] += '0';
                }
                argText = parts[0] + (parts.length > 1 ? 'e' + parts[1] : '');
  
                // Capitalize 'E' if needed.
                if (next == 69) argText = argText.toUpperCase();
  
                // Add sign.
                if (currArg >= 0) {
                  if (flagAlwaysSigned) {
                    argText = '+' + argText;
                  } else if (flagPadSign) {
                    argText = ' ' + argText;
                  }
                }
              }
  
              // Add padding.
              while (argText.length < width) {
                if (flagLeftAlign) {
                  argText += ' ';
                } else {
                  if (flagZeroPad && (argText[0] == '-' || argText[0] == '+')) {
                    argText = argText[0] + '0' + argText.slice(1);
                  } else {
                    argText = (flagZeroPad ? '0' : ' ') + argText;
                  }
                }
              }
  
              // Adjust case.
              if (next < 97) argText = argText.toUpperCase();
  
              // Insert the result into the buffer.
              argText.split('').forEach(function(chr) {
                ret.push(chr.charCodeAt(0));
              });
              break;
            }
            case 's': {
              // String.
              var arg = getNextArg('i8*');
              var argLength = arg ? _strlen(arg) : '(null)'.length;
              if (precisionSet) argLength = Math.min(argLength, precision);
              if (!flagLeftAlign) {
                while (argLength < width--) {
                  ret.push(32);
                }
              }
              if (arg) {
                for (var i = 0; i < argLength; i++) {
                  ret.push(HEAPU8[((arg++)>>0)]);
                }
              } else {
                ret = ret.concat(intArrayFromString('(null)'.substr(0, argLength), true));
              }
              if (flagLeftAlign) {
                while (argLength < width--) {
                  ret.push(32);
                }
              }
              break;
            }
            case 'c': {
              // Character.
              if (flagLeftAlign) ret.push(getNextArg('i8'));
              while (--width > 0) {
                ret.push(32);
              }
              if (!flagLeftAlign) ret.push(getNextArg('i8'));
              break;
            }
            case 'n': {
              // Write the length written so far to the next parameter.
              var ptr = getNextArg('i32*');
              HEAP32[((ptr)>>2)]=ret.length;
              break;
            }
            case '%': {
              // Literal percent sign.
              ret.push(curr);
              break;
            }
            default: {
              // Unknown specifiers remain untouched.
              for (var i = startTextIndex; i < textIndex + 2; i++) {
                ret.push(HEAP8[((i)>>0)]);
              }
            }
          }
          textIndex += 2;
          // TODO: Support a/A (hex float) and m (last error) specifiers.
          // TODO: Support %1${specifier} for arg selection.
        } else {
          ret.push(curr);
          textIndex += 1;
        }
      }
      return ret;
    }
  
  
  
  function __emscripten_traverse_stack(args) {
      if (!args || !args.callee || !args.callee.name) {
        return [null, '', ''];
      }
  
      var funstr = args.callee.toString();
      var funcname = args.callee.name;
      var str = '(';
      var first = true;
      for (var i in args) {
        var a = args[i];
        if (!first) {
          str += ", ";
        }
        first = false;
        if (typeof a === 'number' || typeof a === 'string') {
          str += a;
        } else {
          str += '(' + typeof a + ')';
        }
      }
      str += ')';
      var caller = args.callee.caller;
      args = caller ? caller.arguments : [];
      if (first)
        str = '';
      return [args, funcname, str];
    }
  
  function jsStackTrace() {
      var err = new Error();
      if (!err.stack) {
        // IE10+ special cases: It does have callstack info, but it is only populated if an Error object is thrown,
        // so try that as a special-case.
        try {
          throw new Error(0);
        } catch(e) {
          err = e;
        }
        if (!err.stack) {
          return '(no stack trace available)';
        }
      }
      return err.stack.toString();
    }
  
  function demangle(func) {
      var __cxa_demangle_func = Module['___cxa_demangle'] || Module['__cxa_demangle'];
      assert(__cxa_demangle_func);
      try {
        var s = func;
        if (s.startsWith('__Z'))
          s = s.substr(1);
        var len = lengthBytesUTF8(s)+1;
        var buf = _malloc(len);
        stringToUTF8(s, buf, len);
        var status = _malloc(4);
        var ret = __cxa_demangle_func(buf, 0, 0, status);
        if (HEAP32[((status)>>2)] === 0 && ret) {
          return UTF8ToString(ret);
        }
        // otherwise, libcxxabi failed
      } catch(e) {
        // ignore problems here
      } finally {
        if (buf) _free(buf);
        if (status) _free(status);
        if (ret) _free(ret);
      }
      // failure when using libcxxabi, don't demangle
      return func;
    }function _emscripten_get_callstack_js(flags) {
      var callstack = jsStackTrace();
  
      // Find the symbols in the callstack that corresponds to the functions that report callstack information, and remove everyhing up to these from the output.
      var iThisFunc = callstack.lastIndexOf('_emscripten_log');
      var iThisFunc2 = callstack.lastIndexOf('_emscripten_get_callstack');
      var iNextLine = callstack.indexOf('\n', Math.max(iThisFunc, iThisFunc2))+1;
      callstack = callstack.slice(iNextLine);
  
      // If user requested to see the original source stack, but no source map information is available, just fall back to showing the JS stack.
      if (flags & 8/*EM_LOG_C_STACK*/ && typeof emscripten_source_map === 'undefined') {
        warnOnce('Source map information is not available, emscripten_log with EM_LOG_C_STACK will be ignored. Build with "--pre-js $EMSCRIPTEN/src/emscripten-source-map.min.js" linker flag to add source map loading to code.');
        flags ^= 8/*EM_LOG_C_STACK*/;
        flags |= 16/*EM_LOG_JS_STACK*/;
      }
  
      var stack_args = null;
      if (flags & 128 /*EM_LOG_FUNC_PARAMS*/) {
        // To get the actual parameters to the functions, traverse the stack via the unfortunately deprecated 'arguments.callee' method, if it works:
        stack_args = __emscripten_traverse_stack(arguments);
        while (stack_args[1].indexOf('_emscripten_') >= 0)
          stack_args = __emscripten_traverse_stack(stack_args[0]);
      }
  
      // Process all lines:
      var lines = callstack.split('\n');
      callstack = '';
      var newFirefoxRe = new RegExp('\\s*(.*?)@(.*?):([0-9]+):([0-9]+)'); // New FF30 with column info: extract components of form '       Object._main@http://server.com:4324:12'
      var firefoxRe = new RegExp('\\s*(.*?)@(.*):(.*)(:(.*))?'); // Old FF without column info: extract components of form '       Object._main@http://server.com:4324'
      var chromeRe = new RegExp('\\s*at (.*?) \\\((.*):(.*):(.*)\\\)'); // Extract components of form '    at Object._main (http://server.com/file.html:4324:12)'
  
      for (var l in lines) {
        var line = lines[l];
  
        var jsSymbolName = '';
        var file = '';
        var lineno = 0;
        var column = 0;
  
        var parts = chromeRe.exec(line);
        if (parts && parts.length == 5) {
          jsSymbolName = parts[1];
          file = parts[2];
          lineno = parts[3];
          column = parts[4];
        } else {
          parts = newFirefoxRe.exec(line);
          if (!parts) parts = firefoxRe.exec(line);
          if (parts && parts.length >= 4) {
            jsSymbolName = parts[1];
            file = parts[2];
            lineno = parts[3];
            column = parts[4]|0; // Old Firefox doesn't carry column information, but in new FF30, it is present. See https://bugzilla.mozilla.org/show_bug.cgi?id=762556
          } else {
            // Was not able to extract this line for demangling/sourcemapping purposes. Output it as-is.
            callstack += line + '\n';
            continue;
          }
        }
  
        // Try to demangle the symbol, but fall back to showing the original JS symbol name if not available.
        var cSymbolName = (flags & 32/*EM_LOG_DEMANGLE*/) ? demangle(jsSymbolName) : jsSymbolName;
        if (!cSymbolName) {
          cSymbolName = jsSymbolName;
        }
  
        var haveSourceMap = false;
  
        if (flags & 8/*EM_LOG_C_STACK*/) {
          var orig = emscripten_source_map.originalPositionFor({line: lineno, column: column});
          haveSourceMap = (orig && orig.source);
          if (haveSourceMap) {
            if (flags & 64/*EM_LOG_NO_PATHS*/) {
              orig.source = orig.source.substring(orig.source.replace(/\\/g, "/").lastIndexOf('/')+1);
            }
            callstack += '    at ' + cSymbolName + ' (' + orig.source + ':' + orig.line + ':' + orig.column + ')\n';
          }
        }
        if ((flags & 16/*EM_LOG_JS_STACK*/) || !haveSourceMap) {
          if (flags & 64/*EM_LOG_NO_PATHS*/) {
            file = file.substring(file.replace(/\\/g, "/").lastIndexOf('/')+1);
          }
          callstack += (haveSourceMap ? ('     = '+jsSymbolName) : ('    at '+cSymbolName)) + ' (' + file + ':' + lineno + ':' + column + ')\n';
        }
  
        // If we are still keeping track with the callstack by traversing via 'arguments.callee', print the function parameters as well.
        if (flags & 128 /*EM_LOG_FUNC_PARAMS*/ && stack_args[0]) {
          if (stack_args[1] == jsSymbolName && stack_args[2].length > 0) {
            callstack = callstack.replace(/\s+$/, '');
            callstack += ' with values: ' + stack_args[1] + stack_args[2] + '\n';
          }
          stack_args = __emscripten_traverse_stack(stack_args[0]);
        }
      }
      // Trim extra whitespace at the end of the output.
      callstack = callstack.replace(/\s+$/, '');
      return callstack;
    }function _emscripten_log_js(flags, str) {
      if (flags & 24/*EM_LOG_C_STACK | EM_LOG_JS_STACK*/) {
        str = str.replace(/\s+$/, ''); // Ensure the message and the callstack are joined cleanly with exactly one newline.
        str += (str.length > 0 ? '\n' : '') + _emscripten_get_callstack_js(flags);
      }
  
      if (flags & 1 /*EM_LOG_CONSOLE*/) {
        if (flags & 4 /*EM_LOG_ERROR*/) {
          console.error(str);
        } else if (flags & 2 /*EM_LOG_WARN*/) {
          console.warn(str);
        } else {
          console.log(str);
        }
      } else if (flags & 6 /*EM_LOG_ERROR|EM_LOG_WARN*/) {
        err(str);
      } else {
        out(str);
      }
    }function _emscripten_log(flags, varargs) {
      // Extract the (optionally-existing) printf format specifier field from varargs.
      var format = HEAP32[((varargs)>>2)];
      varargs += 4;
      var str = '';
      if (format) {
        var result = __formatString(format, varargs);
        for(var i = 0 ; i < result.length; ++i) {
          str += String.fromCharCode(result[i]);
        }
      }
      _emscripten_log_js(flags, str);
    }

  function _emscripten_performance_now() {
      return performance.now();
    }

  function _emscripten_request_animation_frame_loop(cb, userData) {
      function tick(timeStamp) {
        if (dynCall_idi(cb, timeStamp, userData)) {
          requestAnimationFrame(tick);
        }
      }
      return requestAnimationFrame(tick);
    }

  
  var JSEvents={keyEvent:0,mouseEvent:0,wheelEvent:0,uiEvent:0,focusEvent:0,deviceOrientationEvent:0,deviceMotionEvent:0,fullscreenChangeEvent:0,pointerlockChangeEvent:0,visibilityChangeEvent:0,touchEvent:0,previousFullscreenElement:null,previousScreenX:null,previousScreenY:null,removeEventListenersRegistered:false,removeAllEventListeners:function() {
        for(var i = JSEvents.eventHandlers.length-1; i >= 0; --i) {
          JSEvents._removeHandler(i);
        }
        JSEvents.eventHandlers = [];
        JSEvents.deferredCalls = [];
      },deferredCalls:[],deferCall:function(targetFunction, precedence, argsList) {
        function arraysHaveEqualContent(arrA, arrB) {
          if (arrA.length != arrB.length) return false;
  
          for(var i in arrA) {
            if (arrA[i] != arrB[i]) return false;
          }
          return true;
        }
        // Test if the given call was already queued, and if so, don't add it again.
        for(var i in JSEvents.deferredCalls) {
          var call = JSEvents.deferredCalls[i];
          if (call.targetFunction == targetFunction && arraysHaveEqualContent(call.argsList, argsList)) {
            return;
          }
        }
        JSEvents.deferredCalls.push({
          targetFunction: targetFunction,
          precedence: precedence,
          argsList: argsList
        });
  
        JSEvents.deferredCalls.sort(function(x,y) { return x.precedence < y.precedence; });
      },removeDeferredCalls:function(targetFunction) {
        for(var i = 0; i < JSEvents.deferredCalls.length; ++i) {
          if (JSEvents.deferredCalls[i].targetFunction == targetFunction) {
            JSEvents.deferredCalls.splice(i, 1);
            --i;
          }
        }
      },canPerformEventHandlerRequests:function() {
        return JSEvents.inEventHandler && JSEvents.currentEventHandler.allowsDeferredCalls;
      },runDeferredCalls:function() {
        if (!JSEvents.canPerformEventHandlerRequests()) {
          return;
        }
        for(var i = 0; i < JSEvents.deferredCalls.length; ++i) {
          var call = JSEvents.deferredCalls[i];
          JSEvents.deferredCalls.splice(i, 1);
          --i;
          call.targetFunction.apply(this, call.argsList);
        }
      },inEventHandler:0,currentEventHandler:null,eventHandlers:[],isInternetExplorer:function() { return navigator.userAgent.indexOf('MSIE') !== -1 || navigator.appVersion.indexOf('Trident/') > 0; },removeAllHandlersOnTarget:function(target, eventTypeString) {
        for(var i = 0; i < JSEvents.eventHandlers.length; ++i) {
          if (JSEvents.eventHandlers[i].target == target && 
            (!eventTypeString || eventTypeString == JSEvents.eventHandlers[i].eventTypeString)) {
             JSEvents._removeHandler(i--);
           }
        }
      },_removeHandler:function(i) {
        var h = JSEvents.eventHandlers[i];
        h.target.removeEventListener(h.eventTypeString, h.eventListenerFunc, h.useCapture);
        JSEvents.eventHandlers.splice(i, 1);
      },registerOrRemoveHandler:function(eventHandler) {
        var jsEventHandler = function jsEventHandler(event) {
          // Increment nesting count for the event handler.
          ++JSEvents.inEventHandler;
          JSEvents.currentEventHandler = eventHandler;
          // Process any old deferred calls the user has placed.
          JSEvents.runDeferredCalls();
          // Process the actual event, calls back to user C code handler.
          eventHandler.handlerFunc(event);
          // Process any new deferred calls that were placed right now from this event handler.
          JSEvents.runDeferredCalls();
          // Out of event handler - restore nesting count.
          --JSEvents.inEventHandler;
        }
        
        if (eventHandler.callbackfunc) {
          eventHandler.eventListenerFunc = jsEventHandler;
          eventHandler.target.addEventListener(eventHandler.eventTypeString, jsEventHandler, eventHandler.useCapture);
          JSEvents.eventHandlers.push(eventHandler);
        } else {
          for(var i = 0; i < JSEvents.eventHandlers.length; ++i) {
            if (JSEvents.eventHandlers[i].target == eventHandler.target
             && JSEvents.eventHandlers[i].eventTypeString == eventHandler.eventTypeString) {
               JSEvents._removeHandler(i--);
             }
          }
        }
      },getBoundingClientRectOrZeros:function(target) {
        return target.getBoundingClientRect ? target.getBoundingClientRect() : { left: 0, top: 0 };
      },pageScrollPos:function() {
        if (pageXOffset > 0 || pageYOffset > 0) {
          return [pageXOffset, pageYOffset];
        }
        if (typeof document.documentElement.scrollLeft !== 'undefined' || typeof document.documentElement.scrollTop !== 'undefined') {
          return [document.documentElement.scrollLeft, document.documentElement.scrollTop];
        }
        return [document.body.scrollLeft|0, document.body.scrollTop|0];
      },getNodeNameForTarget:function(target) {
        if (!target) return '';
        if (target == window) return '#window';
        if (target == screen) return '#screen';
        return (target && target.nodeName) ? target.nodeName : '';
      },tick:function() {
        if (window['performance'] && window['performance']['now']) return window['performance']['now']();
        else return Date.now();
      },fullscreenEnabled:function() {
        return document.fullscreenEnabled || document.mozFullScreenEnabled || document.webkitFullscreenEnabled || document.msFullscreenEnabled;
      }};
  
  
  
  function __maybeCStringToJsString(cString) {
      return cString === cString + 0 ? UTF8ToString(cString) : cString;
    }
  
  var __specialEventTargets=[0, document, window];function __findEventTarget(target) {
      var domElement = __specialEventTargets[target] || document.querySelector(__maybeCStringToJsString(target));
      // TODO: Remove this check in the future, or move it to some kind of debugging mode, because it may be perfectly fine behavior
      // for one to query an event target to test if any DOM element with given CSS selector exists. However for a migration period
      // from old lookup over to new, it is very useful to get diagnostics messages related to a lookup failing.
      if (!domElement) err('No DOM element was found with CSS selector "' + __maybeCStringToJsString(target) + '"');
      return domElement;
    }function __findCanvasEventTarget(target) { return __findEventTarget(target); }function _emscripten_set_canvas_element_size(target, width, height) {
      var canvas = __findCanvasEventTarget(target);
      if (!canvas) return -4;
      canvas.width = width;
      canvas.height = height;
      return 0;
    }

  
  var Fetch={xhrs:[],setu64:function(addr, val) {
      HEAPU32[addr >> 2] = val;
      HEAPU32[addr + 4 >> 2] = (val / 4294967296)|0;
    },staticInit:function() {
      var isMainThread = (typeof ENVIRONMENT_IS_FETCH_WORKER === 'undefined');
  
  
    }};
  
  function __emscripten_fetch_xhr(fetch, onsuccess, onerror, onprogress) {
    var url = HEAPU32[fetch + 8 >> 2];
    if (!url) {
      onerror(fetch, 0, 'no url specified!');
      return;
    }
    var url_ = UTF8ToString(url);
  
    var fetch_attr = fetch + 112;
    var requestMethod = UTF8ToString(fetch_attr);
    if (!requestMethod) requestMethod = 'GET';
    var userData = HEAPU32[fetch_attr + 32 >> 2];
    var fetchAttributes = HEAPU32[fetch_attr + 48 >> 2];
    var timeoutMsecs = HEAPU32[fetch_attr + 52 >> 2];
    var withCredentials = !!HEAPU32[fetch_attr + 56 >> 2];
    var destinationPath = HEAPU32[fetch_attr + 60 >> 2];
    var userName = HEAPU32[fetch_attr + 64 >> 2];
    var password = HEAPU32[fetch_attr + 68 >> 2];
    var requestHeaders = HEAPU32[fetch_attr + 72 >> 2];
    var overriddenMimeType = HEAPU32[fetch_attr + 76 >> 2];
    var dataPtr = HEAPU32[fetch_attr + 80 >> 2];
    var dataLength = HEAPU32[fetch_attr + 84 >> 2];
  
    var fetchAttrLoadToMemory = !!(fetchAttributes & 1);
    var fetchAttrStreamData = !!(fetchAttributes & 2);
    var fetchAttrAppend = !!(fetchAttributes & 8);
    var fetchAttrReplace = !!(fetchAttributes & 16);
    var fetchAttrSynchronous = !!(fetchAttributes & 64);
    var fetchAttrWaitable = !!(fetchAttributes & 128);
  
    var userNameStr = userName ? UTF8ToString(userName) : undefined;
    var passwordStr = password ? UTF8ToString(password) : undefined;
    var overriddenMimeTypeStr = overriddenMimeType ? UTF8ToString(overriddenMimeType) : undefined;
  
    var xhr = new XMLHttpRequest();
    xhr.withCredentials = withCredentials;
    xhr.open(requestMethod, url_, !fetchAttrSynchronous, userNameStr, passwordStr);
    if (!fetchAttrSynchronous) xhr.timeout = timeoutMsecs; // XHR timeout field is only accessible in async XHRs, and must be set after .open() but before .send().
    xhr.url_ = url_; // Save the url for debugging purposes (and for comparing to the responseURL that server side advertised)
    xhr.responseType = fetchAttrStreamData ? 'moz-chunked-arraybuffer' : 'arraybuffer';
  
    if (overriddenMimeType) {
      xhr.overrideMimeType(overriddenMimeTypeStr);
    }
    if (requestHeaders) {
      for(;;) {
        var key = HEAPU32[requestHeaders >> 2];
        if (!key) break;
        var value = HEAPU32[requestHeaders + 4 >> 2];
        if (!value) break;
        requestHeaders += 8;
        var keyStr = UTF8ToString(key);
        var valueStr = UTF8ToString(value);
        xhr.setRequestHeader(keyStr, valueStr);
      }
    }
    Fetch.xhrs.push(xhr);
    var id = Fetch.xhrs.length;
    HEAPU32[fetch + 0 >> 2] = id;
    var data = (dataPtr && dataLength) ? HEAPU8.slice(dataPtr, dataPtr + dataLength) : null;
    // TODO: Support specifying custom headers to the request.
  
    xhr.onload = function(e) {
      var len = xhr.response ? xhr.response.byteLength : 0;
      var ptr = 0;
      var ptrLen = 0;
      if (fetchAttrLoadToMemory && !fetchAttrStreamData) {
        ptrLen = len;
        // The data pointer malloc()ed here has the same lifetime as the emscripten_fetch_t structure itself has, and is
        // freed when emscripten_fetch_close() is called.
        ptr = _malloc(ptrLen);
        HEAPU8.set(new Uint8Array(xhr.response), ptr);
      }
      HEAPU32[fetch + 12 >> 2] = ptr;
      Fetch.setu64(fetch + 16, ptrLen);
      Fetch.setu64(fetch + 24, 0);
      if (len) {
        // If the final XHR.onload handler receives the bytedata to compute total length, report that,
        // otherwise don't write anything out here, which will retain the latest byte size reported in
        // the most recent XHR.onprogress handler.
        Fetch.setu64(fetch + 32, len);
      }
      HEAPU16[fetch + 40 >> 1] = xhr.readyState;
      if (xhr.readyState === 4 && xhr.status === 0) {
        if (len > 0) xhr.status = 200; // If loading files from a source that does not give HTTP status code, assume success if we got data bytes.
        else xhr.status = 404; // Conversely, no data bytes is 404.
      }
      HEAPU16[fetch + 42 >> 1] = xhr.status;
      if (xhr.statusText) stringToUTF8(xhr.statusText, fetch + 44, 64);
      if (xhr.status >= 200 && xhr.status < 300) {
        if (onsuccess) onsuccess(fetch, xhr, e);
      } else {
        if (onerror) onerror(fetch, xhr, e);
      }
    }
    xhr.onerror = function(e) {
      var status = xhr.status; // XXX TODO: Overwriting xhr.status doesn't work here, so don't override anywhere else either.
      if (xhr.readyState == 4 && status == 0) status = 404; // If no error recorded, pretend it was 404 Not Found.
      HEAPU32[fetch + 12 >> 2] = 0;
      Fetch.setu64(fetch + 16, 0);
      Fetch.setu64(fetch + 24, 0);
      Fetch.setu64(fetch + 32, 0);
      HEAPU16[fetch + 40 >> 1] = xhr.readyState;
      HEAPU16[fetch + 42 >> 1] = status;
      if (onerror) onerror(fetch, xhr, e);
    }
    xhr.ontimeout = function(e) {
      if (onerror) onerror(fetch, xhr, e);
    }
    xhr.onprogress = function(e) {
      var ptrLen = (fetchAttrLoadToMemory && fetchAttrStreamData && xhr.response) ? xhr.response.byteLength : 0;
      var ptr = 0;
      if (fetchAttrLoadToMemory && fetchAttrStreamData) {
        // The data pointer malloc()ed here has the same lifetime as the emscripten_fetch_t structure itself has, and is
        // freed when emscripten_fetch_close() is called.
        ptr = _malloc(ptrLen);
        HEAPU8.set(new Uint8Array(xhr.response), ptr);
      }
      HEAPU32[fetch + 12 >> 2] = ptr;
      Fetch.setu64(fetch + 16, ptrLen);
      Fetch.setu64(fetch + 24, e.loaded - ptrLen);
      Fetch.setu64(fetch + 32, e.total);
      HEAPU16[fetch + 40 >> 1] = xhr.readyState;
      if (xhr.readyState >= 3 && xhr.status === 0 && e.loaded > 0) xhr.status = 200; // If loading files from a source that does not give HTTP status code, assume success if we get data bytes
      HEAPU16[fetch + 42 >> 1] = xhr.status;
      if (xhr.statusText) stringToUTF8(xhr.statusText, fetch + 44, 64);
      if (onprogress) onprogress(fetch, xhr, e);
    }
    try {
      xhr.send(data);
    } catch(e) {
      if (onerror) onerror(fetch, xhr, e);
    }
  }
  
  
  var _fetch_work_queue=807456;function __emscripten_get_fetch_work_queue() {
      return _fetch_work_queue;
    }function _emscripten_start_fetch(fetch, successcb, errorcb, progresscb) {
    if (typeof Module !== 'undefined') Module['noExitRuntime'] = true; // If we are the main Emscripten runtime, we should not be closing down.
  
    var fetch_attr = fetch + 112;
    var requestMethod = UTF8ToString(fetch_attr);
    var onsuccess = HEAPU32[fetch_attr + 36 >> 2];
    var onerror = HEAPU32[fetch_attr + 40 >> 2];
    var onprogress = HEAPU32[fetch_attr + 44 >> 2];
    var fetchAttributes = HEAPU32[fetch_attr + 48 >> 2];
    var fetchAttrLoadToMemory = !!(fetchAttributes & 1);
    var fetchAttrStreamData = !!(fetchAttributes & 2);
    var fetchAttrAppend = !!(fetchAttributes & 8);
    var fetchAttrReplace = !!(fetchAttributes & 16);
  
    var reportSuccess = function(fetch, xhr, e) {
      if (onsuccess) dynCall_vi(onsuccess, fetch);
      else if (successcb) successcb(fetch);
    };
  
    var reportProgress = function(fetch, xhr, e) {
      if (onprogress) dynCall_vi(onprogress, fetch);
      else if (progresscb) progresscb(fetch);
    };
  
    var reportError = function(fetch, xhr, e) {
      if (onerror) dynCall_vi(onerror, fetch);
      else if (errorcb) errorcb(fetch);
    };
  
    var performUncachedXhr = function(fetch, xhr, e) {
      __emscripten_fetch_xhr(fetch, reportSuccess, reportError, reportProgress);
    };
  
    __emscripten_fetch_xhr(fetch, reportSuccess, reportError, reportProgress);
    return fetch;
  }

  function _emscripten_throw_string(str) {
      assert(typeof str === 'number');
      throw UTF8ToString(str);
    }

  
  
  var __emscripten_webgl_power_preferences=['default', 'low-power', 'high-performance'];function _emscripten_webgl_do_create_context(target, attributes) {
      assert(attributes);
      var contextAttributes = {};
      var a = attributes >> 2;
      contextAttributes['alpha'] = !!HEAP32[a + (0>>2)];
      contextAttributes['depth'] = !!HEAP32[a + (4>>2)];
      contextAttributes['stencil'] = !!HEAP32[a + (8>>2)];
      contextAttributes['antialias'] = !!HEAP32[a + (12>>2)];
      contextAttributes['premultipliedAlpha'] = !!HEAP32[a + (16>>2)];
      contextAttributes['preserveDrawingBuffer'] = !!HEAP32[a + (20>>2)];
      var powerPreference = HEAP32[a + (24>>2)];
      contextAttributes['powerPreference'] = __emscripten_webgl_power_preferences[powerPreference];
      contextAttributes['failIfMajorPerformanceCaveat'] = !!HEAP32[a + (28>>2)];
      contextAttributes.majorVersion = HEAP32[a + (32>>2)];
      contextAttributes.minorVersion = HEAP32[a + (36>>2)];
      contextAttributes.enableExtensionsByDefault = HEAP32[a + (40>>2)];
      contextAttributes.explicitSwapControl = HEAP32[a + (44>>2)];
      contextAttributes.proxyContextToMainThread = HEAP32[a + (48>>2)];
      contextAttributes.renderViaOffscreenBackBuffer = HEAP32[a + (52>>2)];
  
      var canvas = __findCanvasEventTarget(target);
  
  
  
      if (!canvas) {
        return 0;
      }
  
      if (contextAttributes.explicitSwapControl) {
        return 0;
      }
  
  
      var contextHandle = GL.createContext(canvas, contextAttributes);
      return contextHandle;
    }function _emscripten_webgl_create_context(a0,a1
  ) {
  return _emscripten_webgl_do_create_context(a0,a1);
  }

  
  function _emscripten_webgl_destroy_context_calling_thread(contextHandle) {
      if (GL.currentContext == contextHandle) GL.currentContext = 0;
      GL.deleteContext(contextHandle);
    }function _emscripten_webgl_destroy_context(a0
  ) {
  return _emscripten_webgl_destroy_context_calling_thread(a0);
  }

  function _emscripten_webgl_init_context_attributes(attributes) {
      assert(attributes);
      var a = attributes >> 2;
      for(var i = 0; i < (56>>2); ++i) {
        HEAP32[a+i] = 0;
      }
  
      HEAP32[a + (0>>2)] =
      HEAP32[a + (4>>2)] = 
      HEAP32[a + (12>>2)] = 
      HEAP32[a + (16>>2)] = 
      HEAP32[a + (32>>2)] = 
      HEAP32[a + (40>>2)] = 1;
  
    }

  function _emscripten_webgl_make_context_current(contextHandle) {
      var success = GL.makeContextCurrent(contextHandle);
      return success ? 0 : -5;
    }
  Module["_emscripten_webgl_make_context_current"] = _emscripten_webgl_make_context_current;

  function _exit(status) {
      throw 'exit(' + status + ')';
    }

  function _glActiveTexture(x0) { GLctx['activeTexture'](x0) }

  function _glAttachShader(program, shader) {
      GLctx.attachShader(GL.programs[program],
                              GL.shaders[shader]);
    }

  function _glBindBuffer(target, buffer) {
  
      if (target == 0x88EB /*GL_PIXEL_PACK_BUFFER*/) {
        // In WebGL 2 glReadPixels entry point, we need to use a different WebGL 2 API function call when a buffer is bound to
        // GL_PIXEL_PACK_BUFFER_BINDING point, so must keep track whether that binding point is non-null to know what is
        // the proper API function to call.
        GLctx.currentPixelPackBufferBinding = buffer;
      } else if (target == 0x88EC /*GL_PIXEL_UNPACK_BUFFER*/) {
        // In WebGL 2 gl(Compressed)Tex(Sub)Image[23]D entry points, we need to
        // use a different WebGL 2 API function call when a buffer is bound to
        // GL_PIXEL_UNPACK_BUFFER_BINDING point, so must keep track whether that
        // binding point is non-null to know what is the proper API function to
        // call.
        GLctx.currentPixelUnpackBufferBinding = buffer;
      }
      GLctx.bindBuffer(target, GL.buffers[buffer]);
    }

  function _glBindFramebuffer(target, framebuffer) {
  
      GLctx.bindFramebuffer(target, GL.framebuffers[framebuffer]);
  
    }

  function _glBindRenderbuffer(target, renderbuffer) {
      GLctx.bindRenderbuffer(target, GL.renderbuffers[renderbuffer]);
    }

  function _glBindTexture(target, texture) {
      GLctx.bindTexture(target, GL.textures[texture]);
    }

  function _glBlendColor(x0, x1, x2, x3) { GLctx['blendColor'](x0, x1, x2, x3) }

  function _glBlendEquationSeparate(x0, x1) { GLctx['blendEquationSeparate'](x0, x1) }

  function _glBlendFuncSeparate(x0, x1, x2, x3) { GLctx['blendFuncSeparate'](x0, x1, x2, x3) }

  function _glBufferData(target, size, data, usage) {
      if (GL.currentContext.supportsWebGL2EntryPoints) { // WebGL 2 provides new garbage-free entry points to call to WebGL. Use those always when possible.
        if (data) {
          GLctx.bufferData(target, HEAPU8, usage, data, size);
        } else {
          GLctx.bufferData(target, size, usage);
        }
      } else {
        // N.b. here first form specifies a heap subarray, second form an integer size, so the ?: code here is polymorphic. It is advised to avoid
        // randomly mixing both uses in calling code, to avoid any potential JS engine JIT issues.
        GLctx.bufferData(target, data ? HEAPU8.subarray(data, data+size) : size, usage);
      }
    }

  function _glBufferSubData(target, offset, size, data) {
      if (GL.currentContext.supportsWebGL2EntryPoints) { // WebGL 2 provides new garbage-free entry points to call to WebGL. Use those always when possible.
        GLctx.bufferSubData(target, offset, HEAPU8, data, size);
        return;
      }
      GLctx.bufferSubData(target, offset, HEAPU8.subarray(data, data+size));
    }

  function _glCheckFramebufferStatus(x0) { return GLctx['checkFramebufferStatus'](x0) }

  function _glClear(x0) { GLctx['clear'](x0) }

  function _glClearColor(x0, x1, x2, x3) { GLctx['clearColor'](x0, x1, x2, x3) }

  function _glClearDepthf(x0) { GLctx['clearDepth'](x0) }

  function _glClearStencil(x0) { GLctx['clearStencil'](x0) }

  function _glColorMask(red, green, blue, alpha) {
      GLctx.colorMask(!!red, !!green, !!blue, !!alpha);
    }

  function _glCompileShader(shader) {
      GLctx.compileShader(GL.shaders[shader]);
    }

  function _glCompressedTexImage2D(target, level, internalFormat, width, height, border, imageSize, data) {
      if (GL.currentContext.supportsWebGL2EntryPoints) { // WebGL 2 provides new garbage-free entry points to call to WebGL. Use those always when possible.
        if (GLctx.currentPixelUnpackBufferBinding) {
          GLctx['compressedTexImage2D'](target, level, internalFormat, width, height, border, imageSize, data);
        } else {
          GLctx['compressedTexImage2D'](target, level, internalFormat, width, height, border, HEAPU8, data, imageSize);
        }
        return;
      }
      GLctx['compressedTexImage2D'](target, level, internalFormat, width, height, border, data ? HEAPU8.subarray((data),(data+imageSize)) : null);
    }

  function _glCompressedTexSubImage2D(target, level, xoffset, yoffset, width, height, format, imageSize, data) {
      if (GL.currentContext.supportsWebGL2EntryPoints) { // WebGL 2 provides new garbage-free entry points to call to WebGL. Use those always when possible.
        if (GLctx.currentPixelUnpackBufferBinding) {
          GLctx['compressedTexSubImage2D'](target, level, xoffset, yoffset, width, height, format, imageSize, data);
        } else {
          GLctx['compressedTexSubImage2D'](target, level, xoffset, yoffset, width, height, format, HEAPU8, data, imageSize);
        }
        return;
      }
      GLctx['compressedTexSubImage2D'](target, level, xoffset, yoffset, width, height, format, data ? HEAPU8.subarray((data),(data+imageSize)) : null);
    }

  function _glCreateProgram() {
      var id = GL.getNewId(GL.programs);
      var program = GLctx.createProgram();
      program.name = id;
      GL.programs[id] = program;
      return id;
    }

  function _glCreateShader(shaderType) {
      var id = GL.getNewId(GL.shaders);
      GL.shaders[id] = GLctx.createShader(shaderType);
      return id;
    }

  function _glCullFace(x0) { GLctx['cullFace'](x0) }

  function _glDeleteBuffers(n, buffers) {
      for (var i = 0; i < n; i++) {
        var id = HEAP32[(((buffers)+(i*4))>>2)];
        var buffer = GL.buffers[id];
  
        // From spec: "glDeleteBuffers silently ignores 0's and names that do not
        // correspond to existing buffer objects."
        if (!buffer) continue;
  
        GLctx.deleteBuffer(buffer);
        buffer.name = 0;
        GL.buffers[id] = null;
  
        if (id == GL.currArrayBuffer) GL.currArrayBuffer = 0;
        if (id == GL.currElementArrayBuffer) GL.currElementArrayBuffer = 0;
        if (id == GLctx.currentPixelPackBufferBinding) GLctx.currentPixelPackBufferBinding = 0;
        if (id == GLctx.currentPixelUnpackBufferBinding) GLctx.currentPixelUnpackBufferBinding = 0;
      }
    }

  function _glDeleteFramebuffers(n, framebuffers) {
      for (var i = 0; i < n; ++i) {
        var id = HEAP32[(((framebuffers)+(i*4))>>2)];
        var framebuffer = GL.framebuffers[id];
        if (!framebuffer) continue; // GL spec: "glDeleteFramebuffers silently ignores 0s and names that do not correspond to existing framebuffer objects".
        GLctx.deleteFramebuffer(framebuffer);
        framebuffer.name = 0;
        GL.framebuffers[id] = null;
      }
    }

  function _glDeleteProgram(id) {
      if (!id) return;
      var program = GL.programs[id];
      if (!program) { // glDeleteProgram actually signals an error when deleting a nonexisting object, unlike some other GL delete functions.
        GL.recordError(0x0501 /* GL_INVALID_VALUE */);
        return;
      }
      GLctx.deleteProgram(program);
      program.name = 0;
      GL.programs[id] = null;
      GL.programInfos[id] = null;
    }

  function _glDeleteRenderbuffers(n, renderbuffers) {
      for (var i = 0; i < n; i++) {
        var id = HEAP32[(((renderbuffers)+(i*4))>>2)];
        var renderbuffer = GL.renderbuffers[id];
        if (!renderbuffer) continue; // GL spec: "glDeleteRenderbuffers silently ignores 0s and names that do not correspond to existing renderbuffer objects".
        GLctx.deleteRenderbuffer(renderbuffer);
        renderbuffer.name = 0;
        GL.renderbuffers[id] = null;
      }
    }

  function _glDeleteShader(id) {
      if (!id) return;
      var shader = GL.shaders[id];
      if (!shader) { // glDeleteShader actually signals an error when deleting a nonexisting object, unlike some other GL delete functions.
        GL.recordError(0x0501 /* GL_INVALID_VALUE */);
        return;
      }
      GLctx.deleteShader(shader);
      GL.shaders[id] = null;
    }

  function _glDeleteTextures(n, textures) {
      for (var i = 0; i < n; i++) {
        var id = HEAP32[(((textures)+(i*4))>>2)];
        var texture = GL.textures[id];
        if (!texture) continue; // GL spec: "glDeleteTextures silently ignores 0s and names that do not correspond to existing textures".
        GLctx.deleteTexture(texture);
        texture.name = 0;
        GL.textures[id] = null;
      }
    }

  function _glDepthFunc(x0) { GLctx['depthFunc'](x0) }

  function _glDepthMask(flag) {
      GLctx.depthMask(!!flag);
    }

  function _glDetachShader(program, shader) {
      GLctx.detachShader(GL.programs[program],
                              GL.shaders[shader]);
    }

  function _glDisable(x0) { GLctx['disable'](x0) }

  function _glDisableVertexAttribArray(index) {
      GLctx.disableVertexAttribArray(index);
    }

  function _glDrawArrays(mode, first, count) {
  
      GLctx.drawArrays(mode, first, count);
  
    }


  function _glEnable(x0) { GLctx['enable'](x0) }

  function _glEnableVertexAttribArray(index) {
      GLctx.enableVertexAttribArray(index);
    }

  function _glFlush() { GLctx['flush']() }

  function _glFramebufferRenderbuffer(target, attachment, renderbuffertarget, renderbuffer) {
      GLctx.framebufferRenderbuffer(target, attachment, renderbuffertarget,
                                         GL.renderbuffers[renderbuffer]);
    }

  function _glFramebufferTexture2D(target, attachment, textarget, texture, level) {
      GLctx.framebufferTexture2D(target, attachment, textarget,
                                      GL.textures[texture], level);
    }

  function _glFrontFace(x0) { GLctx['frontFace'](x0) }

  function _glGenBuffers(n, buffers) {
      __glGenObject(n, buffers, 'createBuffer', GL.buffers
        );
    }

  function _glGenFramebuffers(n, ids) {
      __glGenObject(n, ids, 'createFramebuffer', GL.framebuffers
        );
    }

  function _glGenRenderbuffers(n, renderbuffers) {
      __glGenObject(n, renderbuffers, 'createRenderbuffer', GL.renderbuffers
        );
    }

  function _glGenTextures(n, textures) {
      __glGenObject(n, textures, 'createTexture', GL.textures
        );
    }

  function _glGenerateMipmap(x0) { GLctx['generateMipmap'](x0) }

  function _glGetActiveAttrib(program, index, bufSize, length, size, type, name) {
      program = GL.programs[program];
      var info = GLctx.getActiveAttrib(program, index);
      if (!info) return; // If an error occurs, nothing will be written to length, size and type and name.
  
      var numBytesWrittenExclNull = (bufSize > 0 && name) ? stringToUTF8(info.name, name, bufSize) : 0;
      if (length) HEAP32[((length)>>2)]=numBytesWrittenExclNull;
      if (size) HEAP32[((size)>>2)]=info.size;
      if (type) HEAP32[((type)>>2)]=info.type;
    }

  function _glGetActiveUniform(program, index, bufSize, length, size, type, name) {
      program = GL.programs[program];
      var info = GLctx.getActiveUniform(program, index);
      if (!info) return; // If an error occurs, nothing will be written to length, size, type and name.
  
      var numBytesWrittenExclNull = (bufSize > 0 && name) ? stringToUTF8(info.name, name, bufSize) : 0;
      if (length) HEAP32[((length)>>2)]=numBytesWrittenExclNull;
      if (size) HEAP32[((size)>>2)]=info.size;
      if (type) HEAP32[((type)>>2)]=info.type;
    }

  function _glGetAttribLocation(program, name) {
      return GLctx.getAttribLocation(GL.programs[program], UTF8ToString(name));
    }

  function _glGetError() {
      // First return any GL error generated by the emscripten library_webgl.js interop layer.
      if (GL.lastError) {
        var error = GL.lastError;
        GL.lastError = 0/*GL_NO_ERROR*/;
        return error;
      } else
      { // If there were none, return the GL error from the browser GL context.
        return GLctx.getError();
      }
    }

  function _glGetFloatv(name_, p) {
      emscriptenWebGLGet(name_, p, 2);
    }

  function _glGetIntegerv(name_, p) {
      emscriptenWebGLGet(name_, p, 0);
    }

  function _glGetProgramInfoLog(program, maxLength, length, infoLog) {
      var log = GLctx.getProgramInfoLog(GL.programs[program]);
      if (log === null) log = '(unknown error)';
      var numBytesWrittenExclNull = (maxLength > 0 && infoLog) ? stringToUTF8(log, infoLog, maxLength) : 0;
      if (length) HEAP32[((length)>>2)]=numBytesWrittenExclNull;
    }

  function _glGetProgramiv(program, pname, p) {
      if (!p) {
        // GLES2 specification does not specify how to behave if p is a null pointer. Since calling this function does not make sense
        // if p == null, issue a GL error to notify user about it.
        GL.recordError(0x0501 /* GL_INVALID_VALUE */);
        return;
      }
  
      if (program >= GL.counter) {
        GL.recordError(0x0501 /* GL_INVALID_VALUE */);
        return;
      }
  
      var ptable = GL.programInfos[program];
      if (!ptable) {
        GL.recordError(0x0502 /* GL_INVALID_OPERATION */);
        return;
      }
  
      if (pname == 0x8B84) { // GL_INFO_LOG_LENGTH
        var log = GLctx.getProgramInfoLog(GL.programs[program]);
        if (log === null) log = '(unknown error)';
        HEAP32[((p)>>2)]=log.length + 1;
      } else if (pname == 0x8B87 /* GL_ACTIVE_UNIFORM_MAX_LENGTH */) {
        HEAP32[((p)>>2)]=ptable.maxUniformLength;
      } else if (pname == 0x8B8A /* GL_ACTIVE_ATTRIBUTE_MAX_LENGTH */) {
        if (ptable.maxAttributeLength == -1) {
          program = GL.programs[program];
          var numAttribs = GLctx.getProgramParameter(program, 0x8B89/*GL_ACTIVE_ATTRIBUTES*/);
          ptable.maxAttributeLength = 0; // Spec says if there are no active attribs, 0 must be returned.
          for (var i = 0; i < numAttribs; ++i) {
            var activeAttrib = GLctx.getActiveAttrib(program, i);
            ptable.maxAttributeLength = Math.max(ptable.maxAttributeLength, activeAttrib.name.length+1);
          }
        }
        HEAP32[((p)>>2)]=ptable.maxAttributeLength;
      } else if (pname == 0x8A35 /* GL_ACTIVE_UNIFORM_BLOCK_MAX_NAME_LENGTH */) {
        if (ptable.maxUniformBlockNameLength == -1) {
          program = GL.programs[program];
          var numBlocks = GLctx.getProgramParameter(program, 0x8A36/*GL_ACTIVE_UNIFORM_BLOCKS*/);
          ptable.maxUniformBlockNameLength = 0;
          for (var i = 0; i < numBlocks; ++i) {
            var activeBlockName = GLctx.getActiveUniformBlockName(program, i);
            ptable.maxUniformBlockNameLength = Math.max(ptable.maxUniformBlockNameLength, activeBlockName.length+1);
          }
        }
        HEAP32[((p)>>2)]=ptable.maxUniformBlockNameLength;
      } else {
        HEAP32[((p)>>2)]=GLctx.getProgramParameter(GL.programs[program], pname);
      }
    }

  function _glGetShaderInfoLog(shader, maxLength, length, infoLog) {
      var log = GLctx.getShaderInfoLog(GL.shaders[shader]);
      if (log === null) log = '(unknown error)';
      var numBytesWrittenExclNull = (maxLength > 0 && infoLog) ? stringToUTF8(log, infoLog, maxLength) : 0;
      if (length) HEAP32[((length)>>2)]=numBytesWrittenExclNull;
    }

  function _glGetShaderiv(shader, pname, p) {
      if (!p) {
        // GLES2 specification does not specify how to behave if p is a null pointer. Since calling this function does not make sense
        // if p == null, issue a GL error to notify user about it.
        GL.recordError(0x0501 /* GL_INVALID_VALUE */);
        return;
      }
      if (pname == 0x8B84) { // GL_INFO_LOG_LENGTH
        var log = GLctx.getShaderInfoLog(GL.shaders[shader]);
        if (log === null) log = '(unknown error)';
        HEAP32[((p)>>2)]=log.length + 1;
      } else if (pname == 0x8B88) { // GL_SHADER_SOURCE_LENGTH
        var source = GLctx.getShaderSource(GL.shaders[shader]);
        var sourceLength = (source === null || source.length == 0) ? 0 : source.length + 1;
        HEAP32[((p)>>2)]=sourceLength;
      } else {
        HEAP32[((p)>>2)]=GLctx.getShaderParameter(GL.shaders[shader], pname);
      }
    }

  function _glGetString(name_) {
      if (GL.stringCache[name_]) return GL.stringCache[name_];
      var ret;
      switch(name_) {
        case 0x1F03 /* GL_EXTENSIONS */:
          var exts = GLctx.getSupportedExtensions();
          var gl_exts = [];
          for (var i = 0; i < exts.length; ++i) {
            gl_exts.push(exts[i]);
            gl_exts.push("GL_" + exts[i]);
          }
          ret = stringToNewUTF8(gl_exts.join(' '));
          break;
        case 0x1F00 /* GL_VENDOR */:
        case 0x1F01 /* GL_RENDERER */:
        case 0x9245 /* UNMASKED_VENDOR_WEBGL */:
        case 0x9246 /* UNMASKED_RENDERER_WEBGL */:
          var s = GLctx.getParameter(name_);
          if (!s) {
            GL.recordError(0x0500/*GL_INVALID_ENUM*/);
          }
          ret = stringToNewUTF8(s);
          break;
  
        case 0x1F02 /* GL_VERSION */:
          var glVersion = GLctx.getParameter(GLctx.VERSION);
          // return GLES version string corresponding to the version of the WebGL context
          if (GL.currentContext.version >= 2) glVersion = 'OpenGL ES 3.0 (' + glVersion + ')';
          else
          {
            glVersion = 'OpenGL ES 2.0 (' + glVersion + ')';
          }
          ret = stringToNewUTF8(glVersion);
          break;
        case 0x8B8C /* GL_SHADING_LANGUAGE_VERSION */:
          var glslVersion = GLctx.getParameter(GLctx.SHADING_LANGUAGE_VERSION);
          // extract the version number 'N.M' from the string 'WebGL GLSL ES N.M ...'
          var ver_re = /^WebGL GLSL ES ([0-9]\.[0-9][0-9]?)(?:$| .*)/;
          var ver_num = glslVersion.match(ver_re);
          if (ver_num !== null) {
            if (ver_num[1].length == 3) ver_num[1] = ver_num[1] + '0'; // ensure minor version has 2 digits
            glslVersion = 'OpenGL ES GLSL ES ' + ver_num[1] + ' (' + glslVersion + ')';
          }
          ret = stringToNewUTF8(glslVersion);
          break;
        default:
          GL.recordError(0x0500/*GL_INVALID_ENUM*/);
          return 0;
      }
      GL.stringCache[name_] = ret;
      return ret;
    }

  function _glGetUniformLocation(program, name) {
      name = UTF8ToString(name);
  
      var arrayIndex = 0;
      // If user passed an array accessor "[index]", parse the array index off the accessor.
      if (name[name.length - 1] == ']') {
        var leftBrace = name.lastIndexOf('[');
        arrayIndex = name[leftBrace+1] != ']' ? parseInt(name.slice(leftBrace + 1)) : 0; // "index]", parseInt will ignore the ']' at the end; but treat "foo[]" as "foo[0]"
        name = name.slice(0, leftBrace);
      }
  
      var uniformInfo = GL.programInfos[program] && GL.programInfos[program].uniforms[name]; // returns pair [ dimension_of_uniform_array, uniform_location ]
      if (uniformInfo && arrayIndex >= 0 && arrayIndex < uniformInfo[0]) { // Check if user asked for an out-of-bounds element, i.e. for 'vec4 colors[3];' user could ask for 'colors[10]' which should return -1.
        return uniformInfo[1] + arrayIndex;
      } else {
        return -1;
      }
    }

  function _glLinkProgram(program) {
      GLctx.linkProgram(GL.programs[program]);
      GL.populateUniformTable(program);
    }

  function _glPixelStorei(pname, param) {
      if (pname == 0x0cf5 /* GL_UNPACK_ALIGNMENT */) {
        GL.unpackAlignment = param;
      }
      GLctx.pixelStorei(pname, param);
    }

  function _glReadPixels(x, y, width, height, format, type, pixels) {
      if (GL.currentContext.supportsWebGL2EntryPoints) { // WebGL 2 provides new garbage-free entry points to call to WebGL. Use those always when possible.
        if (GLctx.currentPixelPackBufferBinding) {
          GLctx.readPixels(x, y, width, height, format, type, pixels);
        } else {
          GLctx.readPixels(x, y, width, height, format, type, __heapObjectForWebGLType(type), pixels >> (__heapAccessShiftForWebGLType[type]|0));
        }
        return;
      }
      var pixelData = emscriptenWebGLGetTexPixelData(type, format, width, height, pixels, format);
      if (!pixelData) {
        GL.recordError(0x0500/*GL_INVALID_ENUM*/);
        return;
      }
      GLctx.readPixels(x, y, width, height, format, type, pixelData);
    }

  function _glRenderbufferStorage(x0, x1, x2, x3) { GLctx['renderbufferStorage'](x0, x1, x2, x3) }

  function _glScissor(x0, x1, x2, x3) { GLctx['scissor'](x0, x1, x2, x3) }

  function _glShaderSource(shader, count, string, length) {
      var source = GL.getSource(shader, count, string, length);
  
  
      GLctx.shaderSource(GL.shaders[shader], source);
    }

  function _glStencilFuncSeparate(x0, x1, x2, x3) { GLctx['stencilFuncSeparate'](x0, x1, x2, x3) }

  function _glStencilOpSeparate(x0, x1, x2, x3) { GLctx['stencilOpSeparate'](x0, x1, x2, x3) }

  function _glTexImage2D(target, level, internalFormat, width, height, border, format, type, pixels) {
      if (GL.currentContext.supportsWebGL2EntryPoints) {
        // WebGL 2 provides new garbage-free entry points to call to WebGL. Use those always when possible.
        if (GLctx.currentPixelUnpackBufferBinding) {
          GLctx.texImage2D(target, level, internalFormat, width, height, border, format, type, pixels);
        } else if (pixels != 0) {
          GLctx.texImage2D(target, level, internalFormat, width, height, border, format, type, __heapObjectForWebGLType(type), pixels >> (__heapAccessShiftForWebGLType[type]|0));
        } else {
          GLctx.texImage2D(target, level, internalFormat, width, height, border, format, type, null);
        }
        return;
      }
      GLctx.texImage2D(target, level, internalFormat, width, height, border, format, type, pixels ? emscriptenWebGLGetTexPixelData(type, format, width, height, pixels, internalFormat) : null);
    }

  function _glTexParameterf(x0, x1, x2) { GLctx['texParameterf'](x0, x1, x2) }

  function _glTexParameterfv(target, pname, params) {
      var param = HEAPF32[((params)>>2)];
      GLctx.texParameterf(target, pname, param);
    }

  function _glTexParameteri(x0, x1, x2) { GLctx['texParameteri'](x0, x1, x2) }

  function _glTexSubImage2D(target, level, xoffset, yoffset, width, height, format, type, pixels) {
      if (GL.currentContext.supportsWebGL2EntryPoints) {
        // WebGL 2 provides new garbage-free entry points to call to WebGL. Use those always when possible.
        if (GLctx.currentPixelUnpackBufferBinding) {
          GLctx.texSubImage2D(target, level, xoffset, yoffset, width, height, format, type, pixels);
        } else if (pixels != 0) {
          GLctx.texSubImage2D(target, level, xoffset, yoffset, width, height, format, type, __heapObjectForWebGLType(type), pixels >> (__heapAccessShiftForWebGLType[type]|0));
        } else {
          GLctx.texSubImage2D(target, level, xoffset, yoffset, width, height, format, type, null);
        }
        return;
      }
      var pixelData = null;
      if (pixels) pixelData = emscriptenWebGLGetTexPixelData(type, format, width, height, pixels, 0);
      GLctx.texSubImage2D(target, level, xoffset, yoffset, width, height, format, type, pixelData);
    }

  function _glUniform1i(location, v0) {
      GLctx.uniform1i(GL.uniforms[location], v0);
    }

  function _glUniform1iv(location, count, value) {
  
      if (GL.currentContext.supportsWebGL2EntryPoints) { // WebGL 2 provides new garbage-free entry points to call to WebGL. Use those always when possible.
        GLctx.uniform1iv(GL.uniforms[location], HEAP32, value>>2, count);
        return;
      }
  
      GLctx.uniform1iv(GL.uniforms[location], HEAP32.subarray((value)>>2,(value+count*4)>>2));
    }

  function _glUniform4fv(location, count, value) {
  
      if (GL.currentContext.supportsWebGL2EntryPoints) { // WebGL 2 provides new garbage-free entry points to call to WebGL. Use those always when possible.
        GLctx.uniform4fv(GL.uniforms[location], HEAPF32, value>>2, count*4);
        return;
      }
  
      if (4*count <= GL.MINI_TEMP_BUFFER_SIZE) {
        // avoid allocation when uploading few enough uniforms
        var view = GL.miniTempBufferViews[4*count-1];
        for (var i = 0; i < 4*count; i += 4) {
          view[i] = HEAPF32[(((value)+(4*i))>>2)];
          view[i+1] = HEAPF32[(((value)+(4*i+4))>>2)];
          view[i+2] = HEAPF32[(((value)+(4*i+8))>>2)];
          view[i+3] = HEAPF32[(((value)+(4*i+12))>>2)];
        }
      } else
      {
        var view = HEAPF32.subarray((value)>>2,(value+count*16)>>2);
      }
      GLctx.uniform4fv(GL.uniforms[location], view);
    }

  function _glUniformMatrix3fv(location, count, transpose, value) {
  
      if (GL.currentContext.supportsWebGL2EntryPoints) { // WebGL 2 provides new garbage-free entry points to call to WebGL. Use those always when possible.
        GLctx.uniformMatrix3fv(GL.uniforms[location], !!transpose, HEAPF32, value>>2, count*9);
        return;
      }
  
      if (9*count <= GL.MINI_TEMP_BUFFER_SIZE) {
        // avoid allocation when uploading few enough uniforms
        var view = GL.miniTempBufferViews[9*count-1];
        for (var i = 0; i < 9*count; i += 9) {
          view[i] = HEAPF32[(((value)+(4*i))>>2)];
          view[i+1] = HEAPF32[(((value)+(4*i+4))>>2)];
          view[i+2] = HEAPF32[(((value)+(4*i+8))>>2)];
          view[i+3] = HEAPF32[(((value)+(4*i+12))>>2)];
          view[i+4] = HEAPF32[(((value)+(4*i+16))>>2)];
          view[i+5] = HEAPF32[(((value)+(4*i+20))>>2)];
          view[i+6] = HEAPF32[(((value)+(4*i+24))>>2)];
          view[i+7] = HEAPF32[(((value)+(4*i+28))>>2)];
          view[i+8] = HEAPF32[(((value)+(4*i+32))>>2)];
        }
      } else
      {
        var view = HEAPF32.subarray((value)>>2,(value+count*36)>>2);
      }
      GLctx.uniformMatrix3fv(GL.uniforms[location], !!transpose, view);
    }

  function _glUniformMatrix4fv(location, count, transpose, value) {
  
      if (GL.currentContext.supportsWebGL2EntryPoints) { // WebGL 2 provides new garbage-free entry points to call to WebGL. Use those always when possible.
        GLctx.uniformMatrix4fv(GL.uniforms[location], !!transpose, HEAPF32, value>>2, count*16);
        return;
      }
  
      if (16*count <= GL.MINI_TEMP_BUFFER_SIZE) {
        // avoid allocation when uploading few enough uniforms
        var view = GL.miniTempBufferViews[16*count-1];
        for (var i = 0; i < 16*count; i += 16) {
          view[i] = HEAPF32[(((value)+(4*i))>>2)];
          view[i+1] = HEAPF32[(((value)+(4*i+4))>>2)];
          view[i+2] = HEAPF32[(((value)+(4*i+8))>>2)];
          view[i+3] = HEAPF32[(((value)+(4*i+12))>>2)];
          view[i+4] = HEAPF32[(((value)+(4*i+16))>>2)];
          view[i+5] = HEAPF32[(((value)+(4*i+20))>>2)];
          view[i+6] = HEAPF32[(((value)+(4*i+24))>>2)];
          view[i+7] = HEAPF32[(((value)+(4*i+28))>>2)];
          view[i+8] = HEAPF32[(((value)+(4*i+32))>>2)];
          view[i+9] = HEAPF32[(((value)+(4*i+36))>>2)];
          view[i+10] = HEAPF32[(((value)+(4*i+40))>>2)];
          view[i+11] = HEAPF32[(((value)+(4*i+44))>>2)];
          view[i+12] = HEAPF32[(((value)+(4*i+48))>>2)];
          view[i+13] = HEAPF32[(((value)+(4*i+52))>>2)];
          view[i+14] = HEAPF32[(((value)+(4*i+56))>>2)];
          view[i+15] = HEAPF32[(((value)+(4*i+60))>>2)];
        }
      } else
      {
        var view = HEAPF32.subarray((value)>>2,(value+count*64)>>2);
      }
      GLctx.uniformMatrix4fv(GL.uniforms[location], !!transpose, view);
    }

  function _glUseProgram(program) {
      GLctx.useProgram(GL.programs[program]);
    }

  function _glVertexAttribPointer(index, size, type, normalized, stride, ptr) {
      GLctx.vertexAttribPointer(index, size, type, !!normalized, stride, ptr);
    }

  function _glViewport(x0, x1, x2, x3) { GLctx['viewport'](x0, x1, x2, x3) }

  function _js_html_audioCheckLoad(audioClipIdx) {
          var WORKING_ON_IT = 0;
          var SUCCESS = 1;
          var FAILED = 2;
  
          if (!this.audioContext || audioClipIdx < 0)
              return FAILED;
          if (this.audioBuffers[audioClipIdx] == null)
              return FAILED;
          if (this.audioBuffers[audioClipIdx] === 'loading')
              return WORKING_ON_IT; 
          if (this.audioBuffers[audioClipIdx] === 'error')
              return FAILED;
          return SUCCESS;
      }

  function _js_html_audioFree(audioClipIdx) {
          var audioBuffer = this.audioBuffers[audioClipIdx];
          if (!audioBuffer)
              return;
  
          for (var i = 0; i < this.audioSources.length; ++i) {
              var sourceNode = this.audioSources[i];
              if (sourceNode && sourceNode.buffer === audioBuffer)
                  sourceNode.stop();
          }
  
          this.audioBuffers[audioClipIdx] = null;
      }

  function _js_html_audioIsPlaying(audioSourceIdx) {
          if (!this.audioContext || audioSourceIdx < 0)
              return false;
  
          if (this.audioSources[audioSourceIdx] == null)
              return false;
  
          return this.audioSources[audioSourceIdx].isPlaying;
      }

  function _js_html_audioIsUnlocked() {
          return this.unlocked;
      }

  function _js_html_audioPause() {
          if (!this.audioContext)
              return;
  
          this.audioContext.suspend();
      }

  function _js_html_audioPlay(audioClipIdx, audioSourceIdx, volume, loop) 
      {
          if (!this.audioContext || audioClipIdx < 0 || audioSourceIdx < 0)
              return false;
  
          if (this.audioContext.state !== 'running')
              return false;
  
          // require audio buffer to be loaded
          var srcBuffer = this.audioBuffers[audioClipIdx];
          if (!srcBuffer || typeof srcBuffer === 'string')
              return false;
  
          // create audio source node
          var sourceNode = this.audioContext.createBufferSource();
          sourceNode.buffer = srcBuffer;
  
          // create gain node if needed
          if (volume !== 1.0) {
              var gainNode = this.audioContext.createGain();
              gainNode.gain.setValueAtTime(volume, 0);
              sourceNode.connect(gainNode);
              gainNode.connect(this.audioContext.destination);
          } else {
              sourceNode.connect(this.audioContext.destination);
          }
  
          // loop value
          sourceNode.loop = loop;
  
          // stop audio source node if it is already playing
          this.stop(audioSourceIdx, true);
  
          // store audio source node
          this.audioSources[audioSourceIdx] = sourceNode;
  
          // on ended event
          var self = this;
          sourceNode.onended = function (event) {
              self.stop(audioSourceIdx, false);
              //console.log("onended callback");
              sourceNode.isPlaying = false;
          };
  
          // play audio source
          sourceNode.start();
          sourceNode.isPlaying = true;
          //console.log("[Audio] playing " + audioSourceIdx);
          return true;
      }

  function _js_html_audioResume() {
          if (!this.audioContext || typeof this.audioContext.resume !== 'function')
              return;
  
          this.audioContext.resume();
      }

  function _js_html_audioStartLoadFile(audioClipName, audioClipIdx) 
      {
          if (!this.audioContext || audioClipIdx < 0)
              return -1;
  
          audioClipName = UTF8ToString(audioClipName);
  
          var url = audioClipName;
          if (url.substring(0, 9) === "ut-asset:")
              url = UT_ASSETS[url.substring(9)];
  
          var self = this;
          var request = new XMLHttpRequest();
  
          self.audioBuffers[audioClipIdx] = 'loading';
          request.open('GET', url, true);
          request.responseType = 'arraybuffer';
          request.onload =
              function () {
                  self.audioContext.decodeAudioData(request.response, function (buffer) {
                      self.audioBuffers[audioClipIdx] = buffer;
                  });
              };
          request.onerror =
              function () {
                  self.audioBuffers[audioClipIdx] = 'error';
              };
          try {
              request.send();
              //Module._AudioService_AudioClip_OnLoading(entity,audioClipIdx);
          } catch (e) {
              // LG Nexus 5 + Android OS 4.4.0 + Google Chrome 30.0.1599.105 browser
              // odd behavior: If loading from base64-encoded data URI and the
              // format is unsupported, request.send() will immediately throw and
              // not raise the failure at .onerror() handler. Therefore catch
              // failures also eagerly from .send() above.
              self.audioBuffers[audioClipIdx] = 'error';
          }
  
          return audioClipIdx;
      }

  function _js_html_audioStop(audioSourceIdx, dostop) {
          if (!this.audioContext || audioSourceIdx < 0)
              return;
  
          // retrieve audio source node
          var sourceNode = this.audioSources[audioSourceIdx];
          if (!sourceNode)
              return;
  
          // forget audio source node
          sourceNode.onended = null;
          this.audioSources[audioSourceIdx] = null;
  
          // stop audio source
          if (dostop) {
              sourceNode.stop();
              sourceNode.isPlaying = false;
              //console.log("[Audio] stopping " + audioSourceIdx);
          }
      }

  function _js_html_audioUnlock() {
          var self = this;
          if (self.unlocked || !self.audioContext ||
              typeof self.audioContext.resume !== 'function')
              return;
  
          // setup a touch start listener to attempt an unlock in
          document.addEventListener('click', ut._HTML.unlock, true);
          document.addEventListener('touchstart', ut._HTML.unlock, true);
          document.addEventListener('touchend', ut._HTML.unlock, true);
          document.addEventListener('keydown', ut._HTML.unlock, true);
          document.addEventListener('keyup', ut._HTML.unlock, true);
      }

  function _js_html_checkLoadImage(idx) {
      var img = ut._HTML.images[idx];
  
      if ( img.loaderror ) {
        return 2;
      }
  
      if (img.image) {
        if (!img.image.complete || !img.image.naturalWidth || !img.image.naturalHeight)
          return 0; // null - not yet loaded
      }
  
      if (img.mask) {
        if (!img.mask.complete || !img.mask.naturalWidth || !img.mask.naturalHeight)
          return 0; // null - not yet loaded
      }
  
      return 1; // ok
    }

  function _js_html_finishLoadImage(idx, wPtr, hPtr, alphaPtr) {
      var img = ut._HTML.images[idx];
      // check three combinations of mask and image
      if (img.image && img.mask) { // image and mask, merge mask into image 
        var width = img.image.naturalWidth;
        var height = img.image.naturalHeight;
        var maskwidth = img.mask.naturalWidth;
        var maskheight = img.mask.naturalHeight;
  
        // construct the final image
        var cvscolor = document.createElement('canvas');
        cvscolor.width = width;
        cvscolor.height = height;
        var cxcolor = cvscolor.getContext('2d');
        cxcolor.globalCompositeOperation = 'copy';
        cxcolor.drawImage(img.image, 0, 0);
  
        var cvsalpha = document.createElement('canvas');
        cvsalpha.width = width;
        cvsalpha.height = height;
        var cxalpha = cvsalpha.getContext('2d');
        cxalpha.globalCompositeOperation = 'copy';
        cxalpha.drawImage(img.mask, 0, 0, width, height);
  
        var colorBits = cxcolor.getImageData(0, 0, width, height);
        var alphaBits = cxalpha.getImageData(0, 0, width, height);
        var cdata = colorBits.data, adata = alphaBits.data;
        var sz = width * height;
        for (var i = 0; i < sz; i++)
          cdata[(i<<2) + 3] = adata[i<<2];
        cxcolor.putImageData(colorBits, 0, 0);
  
        img.image = cvscolor;
        img.image.naturalWidth = width;
        img.image.naturalHeight = height; 
        img.hasAlpha = true; 
      } else if (!img.image && img.mask) { // mask only, create image
        var width = img.mask.naturalWidth;
        var height = img.mask.naturalHeight;
  
        // construct the final image: copy R to all channels 
        var cvscolor = document.createElement('canvas');
        cvscolor.width = width;
        cvscolor.height = height;
        var cxcolor = cvscolor.getContext('2d');
        cxcolor.globalCompositeOperation = 'copy';
        cxcolor.drawImage(img.mask, 0, 0);
  
        var colorBits = cxcolor.getImageData(0, 0, width, height);
        var cdata = colorBits.data;
        var sz = width * height;
        for (var i = 0; i < sz; i++) {
          cdata[(i<<2) + 1] = cdata[i<<2];
          cdata[(i<<2) + 2] = cdata[i<<2];
          cdata[(i<<2) + 3] = cdata[i<<2];
        }
        cxcolor.putImageData(colorBits, 0, 0);
  
        img.image = cvscolor;
        img.image.naturalWidth = width;
        img.image.naturalHeight = height; 
        img.hasAlpha = true; 
      } // else img.image only, nothing else to do here
  
      // done, return valid size and hasAlpha
      HEAP32[wPtr>>2] = img.image.naturalWidth;
      HEAP32[hPtr>>2] = img.image.naturalHeight;
      HEAP32[alphaPtr>>2] = img.hasAlpha;
    }

  function _js_html_freeImage(idx) {
      ut._HTML.images[idx] = null;
    }

  function _js_html_getCanvasSize(wPtr, hPtr) {
      var html = ut._HTML;
      HEAP32[wPtr>>2] = html.canvasElement.width | 0;
      HEAP32[hPtr>>2] = html.canvasElement.height | 0;
    }

  function _js_html_getFrameSize(wPtr, hPtr) {
      HEAP32[wPtr>>2] = window.innerWidth | 0;
      HEAP32[hPtr>>2] = window.innerHeight | 0;
    }

  function _js_html_getScreenSize(wPtr, hPtr) {
      HEAP32[wPtr>>2] = screen.width | 0;
      HEAP32[hPtr>>2] = screen.height | 0;
    }

  function _js_html_imageToMemory(idx, w, h, dest) {
      // TODO: there could be a fast(ish) path for webgl to get gl to directly write to
      // dest when reading from render targets
      var cvs = ut._HTML.readyCanvasForReadback(idx,w,h);
      if (!cvs)
        return 0;
      var cx = cvs.getContext('2d');
      var imd = cx.getImageData(0, 0, w, h);
      HEAPU8.set(imd.data,dest);
      return 1;
    }

  function _js_html_init() {
      ut = ut || {};
      ut._HTML = ut._HTML || {};
  
      var html = ut._HTML;
      html.visible = true;
      html.focused = true;
    }

  function _js_html_initAudio() {
          
          ut = ut || {};
          ut._HTML = ut._HTML || {};
  
          ut._HTML.unlock = function() {
          // call this method on touch start to create and play a buffer, then check
          // if the audio actually played to determine if audio has now been
          // unlocked on iOS, Android, etc.
              if (!self.audioContext)
                  return;
  
              // fix Android can not play in suspend state
              self.audioContext.resume();
  
              // create an empty buffer
              var source = self.audioContext.createBufferSource();
              source.buffer = self.audioContext.createBuffer(1, 1, 22050);
              source.connect(self.audioContext.destination);
  
              // play the empty buffer
              if (typeof source.start === 'undefined') {
                  source.noteOn(0);
              } else {
                  source.start(0);
              }
  
              // calling resume() on a stack initiated by user gesture is what
              // actually unlocks the audio on Android Chrome >= 55
              self.audioContext.resume();
  
              // setup a timeout to check that we are unlocked on the next event
              // loop
              source.onended = function () {
                  source.disconnect(0);
  
                  // update the unlocked state and prevent this check from happening
                  // again
                  self.unlocked = true;
                  //console.log("[Audio] unlocked");
  
                  // remove the touch start listener
                  document.removeEventListener('click', ut._HTML.unlock, true);
                  document.removeEventListener('touchstart', ut._HTML.unlock, true);
                  document.removeEventListener('touchend', ut._HTML.unlock, true);
                  document.removeEventListener('keydown', ut._HTML.unlock, true);
                  document.removeEventListener('keyup', ut._HTML.unlock, true);
              };
          };
  
          // audio initialization
          if (!window.AudioContext && !window.webkitAudioContext)
              return false;
  
          var audioContext =
              new (window.AudioContext || window.webkitAudioContext)();
          if (!audioContext)
              return false;
  
          this.audioContext = audioContext;
          this.audioBuffers = {};
          this.audioSources = {};
  
          // try to unlock audio
          this.unlocked = false;
          var navigator = (typeof window !== 'undefined' && window.navigator)
              ? window.navigator
              : null;
          var isMobile = /iPhone|iPad|iPod|Android|BlackBerry|BB10|Silk|Mobi/i.test(
              navigator && navigator.userAgent);
          var isTouch = !!(isMobile ||
              (navigator && navigator.maxTouchPoints > 0) ||
              (navigator && navigator.msMaxTouchPoints > 0));
          if (this.audioContext.state !== 'running' || isMobile || isTouch) {
              ut._HTML.unlock();
          } else {
              this.unlocked = true;
          }
          //console.log("[Audio] initialized " + (this.unlocked ? "unlocked" : "locked"));
          return true;
      }

  function _js_html_initImageLoading() {
      ut = ut || {};
      ut._HTML = ut._HTML || {};
  
      ut._HTML.images = [null];             // referenced by drawable, direct index to loaded image. maps 1:1 to Image2D component
                                      // { image, mask, loaderror, hasAlpha}
      ut._HTML.tintedSprites = [null];      // referenced by drawable, sub-sprite with colorization
                                      // { image, pattern }
      ut._HTML.tintedSpritesFreeList = [];
  
      // local helper functions
      ut._HTML.initImage = function(idx ) {
        ut._HTML.images[idx] = {
          image: null,
          mask: null,
          loaderror: false,
          hasAlpha: true,
          glTexture: null,
          glDisableSmoothing: false
        };
      };
  
      ut._HTML.ensureImageIsReadable = function (idx, w, h) {
        if (ut._HTML.canvasMode == 'webgl2' || ut._HTML.canvasMode == 'webgl') {
          var gl = ut._HTML.canvasContext;
          if (ut._HTML.images[idx].isrt) { // need to readback
            if (!ut._HTML.images[idx].glTexture)
              return false;
            // create fbo, read back bytes, write to image pixels
            var pixels = new Uint8Array(w*h*4);
            var fbo = gl.createFramebuffer();
            gl.bindFramebuffer(gl.FRAMEBUFFER, fbo);
            gl.framebufferTexture2D(gl.FRAMEBUFFER, gl.COLOR_ATTACHMENT0, gl.TEXTURE_2D, ut._HTML.images[idx].glTexture, 0);
            gl.viewport(0,0,w,h);
            if (gl.checkFramebufferStatus(gl.FRAMEBUFFER)==gl.FRAMEBUFFER_COMPLETE) {
              gl.readPixels(0, 0, w, h, gl.RGBA, gl.UNSIGNED_BYTE, pixels);
            } else {
              console.log("Warning, can not read back from WebGL framebuffer.");
              gl.bindFramebuffer(gl.FRAMEBUFFER, null);
              gl.deleteFramebuffer(fbo);
              return false;
            }
            // restore default fbo
            gl.bindFramebuffer(gl.FRAMEBUFFER, null);
            gl.deleteFramebuffer(fbo);
            // put pixels onto an image
            var canvas = document.createElement('canvas');
            canvas.width = w;
            canvas.height = h;
            var cx = canvas.getContext('2d');
            var imd = cx.createImageData(w, h);
            imd.data.set(pixels);
            cx.putImageData(imd,0,0);
            ut._HTML.images[idx].image = canvas;
            return true;
          }
        }
        if (ut._HTML.images[idx].isrt)
          return ut._HTML.images[idx].image && ut._HTML.images[idx].width==w && ut._HTML.images[idx].height==h;
        else
          return ut._HTML.images[idx].image && ut._HTML.images[idx].image.naturalWidth===w && ut._HTML.images[idx].image.naturalHeight===h;
      };
  
      ut._HTML.readyCanvasForReadback = function (idx, w, h) {
        if (!ut._HTML.ensureImageIsReadable(idx,w,h)) 
          return null;
        if (ut._HTML.images[idx].image instanceof HTMLCanvasElement) {
          // directly use canvas if the image is already a canvas (RTT case)
          return ut._HTML.images[idx].image;
        } else {
          // otherwise copy to a temp canvas
          var cvs = document.createElement('canvas');
          cvs.width = w;
          cvs.height = h;
          var cx = cvs.getContext('2d');
          var srcimg = ut._HTML.images[idx].image;
          cx.globalCompositeOperation = 'copy';
          cx.drawImage(srcimg, 0, 0, w, h);
          return cvs;
        }
      };
  
      ut._HTML.loadWebPFallback = function(url, idx) {
        function decode_base64(base64) {
          var size = base64.length;
          while (base64.charCodeAt(size - 1) == 0x3D)
            size--;
          var data = new Uint8Array(size * 3 >> 2);
          for (var c, cPrev = 0, s = 6, d = 0, b = 0; b < size; cPrev = c, s = s + 2 & 7) {
            c = base64.charCodeAt(b++);
            c = c >= 0x61 ? c - 0x47 : c >= 0x41 ? c - 0x41 : c >= 0x30 ? c + 4 : c == 0x2F ? 0x3F : 0x3E;
            if (s < 6)
              data[d++] = cPrev << 2 + s | c >> 4 - s;
          }
          return data;
        }
        if(!url)
          return false;
        if (!(typeof WebPDecoder == "object"))
          return false; // no webp fallback installed, let it fail on it's own
        if (WebPDecoder.nativeSupport)
          return false; // regular loading
        var webpCanvas;
        var webpPrefix = "data:image/webp;base64,";
        if (!url.lastIndexOf(webpPrefix, 0)) { // data url 
          webpCanvas = document.createElement("canvas");
          WebPDecoder.decode(decode_base64(url.substring(webpPrefix.length)), webpCanvas);
          webpCanvas.naturalWidth = webpCanvas.width;
          webpCanvas.naturalHeight = webpCanvas.height;
          webpCanvas.complete = true;
          ut._HTML.initImage(idx);
          ut._HTML.images[idx].image = webpCanvas;
          return true;
        }
        if (url.lastIndexOf("data:image/", 0) && url.match(/\.webp$/i)) {
          webpCanvas = document.createElement("canvas");
          webpCanvas.naturalWidth = 0;
          webpCanvas.naturalHeight = 0;
          webpCanvas.complete = false;
          ut._HTML.initImage(idx);
          ut._HTML.images[idx].image = webpCanvas;
          var webpRequest = new XMLHttpRequest();
          webpRequest.responseType = "arraybuffer";
          webpRequest.open("GET", url);
          webpRequest.onerror = function () {
            ut._HTML.images[idx].loaderror = true;
          };
          webpRequest.onload = function () {
            WebPDecoder.decode(new Uint8Array(webpRequest.response), webpCanvas);
            webpCanvas.naturalWidth = webpCanvas.width;
            webpCanvas.naturalHeight = webpCanvas.height;
            webpCanvas.complete = true;
          };
          webpRequest.send();
          return true;
        }
        return false; 
      };
  
    }

  function _js_html_loadImage(colorName, maskName) {
      colorName = colorName ? UTF8ToString(colorName) : null;
      maskName = maskName ? UTF8ToString(maskName) : null;
  
      // rewrite some special urls 
      if (colorName == "::white1x1") {
        colorName = "data:image/gif;base64,R0lGODlhAQABAIAAAP7//wAAACH5BAAAAAAALAAAAAABAAEAAAICRAEAOw==";
      } else if (colorName && colorName.substring(0, 9) == "ut-asset:") {
        colorName = UT_ASSETS[colorName.substring(9)];
      }
      if (maskName && maskName.substring(0, 9) == "ut-asset:") {
        maskName = UT_ASSETS[maskName.substring(9)];
      }
  
      // grab first free index
      var idx;
      for (var i = 1; i <= ut._HTML.images.length; i++) {
        if (!ut._HTML.images[i]) {
          idx = i;
          break;
        }
      }
      ut._HTML.initImage(idx);
  
      // webp fallback if needed (extra special case)
      if (ut._HTML.loadWebPFallback(colorName, idx) )
        return idx;
  
      // start actual load
      if (colorName) {
        var imgColor = new Image();
        var isjpg = !!colorName.match(/\.jpe?g$/i);
        ut._HTML.images[idx].image = imgColor;
        ut._HTML.images[idx].hasAlpha = !isjpg;
        imgColor.onerror = function() { ut._HTML.images[idx].loaderror = true; };
        imgColor.src = colorName;
      }
  
      if (maskName) {
        var imgMask = new Image();
        ut._HTML.images[idx].mask = imgMask;
        ut._HTML.images[idx].hasAlpha = true;
        imgMask.onerror = function() { ut._HTML.images[idx].loaderror = true; };
        imgMask.src = maskName;
      }
  
      return idx; 
    }

  function _js_html_setCanvasSize(width, height) {
      if (!width>0 || !height>0)
          throw "Bad canvas size at init.";
      var canvas = ut._HTML.canvasElement;
      if (!canvas) {
        // take possible user element
        canvas = document.getElementById("UT_CANVAS");
        if (canvas)
          console.log('Using user UT_CANVAS element.');
      } 
      if (!canvas) {
        canvas = document.createElement("canvas");
        canvas.setAttribute("id", "UT_CANVAS");
        canvas.setAttribute("style", "touch-action: none;");
        canvas.setAttribute("tabindex", "1");
        if (document.body) {
          document.body.style.margin = "0px";
          document.body.style.border = "0";
          document.body.style.overflow = "hidden"; // disable scrollbars
          document.body.style.display = "block";   // no floating content on sides
          document.body.insertBefore(canvas, document.body.firstChild);
        } else {
          document.documentElement.appendChild(canvas);
        }
      }
  
      ut._HTML.canvasElement = canvas;
  
      canvas.width = width;
      canvas.height = height;
  
      ut._HTML.canvasMode = 'bgfx';
  
      canvas.addEventListener("webglcontextlost", function(event) { event.preventDefault(); }, false);
      window.addEventListener("focus", function(event) { ut._HTML.focus = true; } );
      window.addEventListener("blur", function(event) { ut._HTML.focus = false; } );
  
      canvas.focus();
      return true;
    }

  function _js_inputGetCanvasLost() {
          // need to reset all input state in case the canvas element changed and re-init input
          var inp = ut._HTML.input;        
          var canvas = ut._HTML.canvasElement;    
          return canvas != inp.canvas; 
      }

  function _js_inputGetFocusLost() {
          var inp = ut._HTML.input;
          // need to reset all input state in that case
          if ( inp.focusLost ) {
              inp.focusLost = false; 
              return true; 
          }
          return false;
      }

  function _js_inputGetKeyStream(maxLen,destPtr) {
          var inp = ut._HTML.input;
          return inp.getStream(inp.keyStream,maxLen,destPtr);            
      }

  function _js_inputGetMouseStream(maxLen,destPtr) {
          var inp = ut._HTML.input;
          return inp.getStream(inp.mouseStream,maxLen,destPtr);
      }

  function _js_inputGetTouchStream(maxLen,destPtr) {
          var inp = ut._HTML.input;
          return inp.getStream(inp.touchStream,maxLen,destPtr);        
      }

  function _js_inputInit() {
          ut._HTML = ut._HTML || {};
          ut._HTML.input = {}; // reset input object, reinit on canvas change
          var inp = ut._HTML.input; 
          var canvas = ut._HTML.canvasElement;
          
          if (!canvas) 
              return false;
              
          inp.getStream = function(stream,maxLen,destPtr) {
              destPtr>>=2;
              var l = stream.length;
              if ( l>maxLen ) l = maxLen;
              for ( var i=0; i<l; i++ )
                  HEAP32[destPtr+i] = stream[i];
              return l;
          };
              
          inp.mouseEventFn = function(ev) {
              var inp = ut._HTML.input;
              var eventType;
              var buttons = 0;
              if (ev.type == "mouseup") { eventType = 0; buttons = ev.button; }
              else if (ev.type == "mousedown") { eventType = 1; buttons = ev.button; }
              else if (ev.type == "mousemove") { eventType = 2; }
              else return;
              var rect = inp.canvas.getBoundingClientRect();
              var x = ev.pageX - rect.left;
              var y = rect.bottom - 1 - ev.pageY; // (rect.bottom - rect.top) - 1 - (ev.pageY - rect.top);
              inp.mouseStream.push(eventType|0);
              inp.mouseStream.push(buttons|0);
              inp.mouseStream.push(x|0);
              inp.mouseStream.push(y|0);
              ev.preventDefault(); 
              ev.stopPropagation();
          };
          
          inp.touchEventFn = function(ev) {
              var inp = ut._HTML.input;
              var eventType, x, y, touch, touches = ev.changedTouches;
              var buttons = 0;
              var eventType;
              if (ev.type == "touchstart") eventType = 1;
              else if (ev.type == "touchend") eventType = 0;
              else if (ev.type == "touchcanceled") eventType = 3;
              else eventType = 2;
              var rect = inp.canvas.getBoundingClientRect();
              for (var i = 0; i < touches.length; ++i) {
                  var t = touches[i];
                  var x = t.pageX - rect.left;
                  var y = rect.bottom - 1 - t.pageY; // (rect.bottom - rect.top) - 1 - (t.pageY - rect.top);
                  inp.touchStream.push(eventType|0);
                  inp.touchStream.push(t.identifier|0);
                  inp.touchStream.push(x|0);
                  inp.touchStream.push(y|0);
              }
              ev.preventDefault();
              ev.stopPropagation();
          };       
  
          inp.keyEventFn = function(ev) {
              var eventType;
              if (ev.type == "keydown") eventType = 1;
              else if (ev.type == "keyup") eventType = 0;
              else return;
              inp.keyStream.push(eventType|0);
              inp.keyStream.push(ev.keyCode|0);
          };        
  
          inp.clickEventFn = function() {
              // ensures we can regain focus if focus is lost
              this.focus(); 
          };        
  
          inp.focusoutEventFn = function() {
              var inp = ut._HTML.input;
              inp.focusLost = true;
          };
          
          inp.mouseStream = [];
          inp.keyStream = [];  
          inp.touchStream = [];
          inp.canvas = canvas; 
          inp.focusLost = false;
          
          // @TODO: handle multitouch
          // Pointer events get delivered on Android Chrome with pageX/pageY
          // in a coordinate system that I can't figure out.  So don't use
          // them at all.
          //events["pointerdown"] = events["pointerup"] = events["pointermove"] = html.pointerEventFn;
          var events = {}
          events["keydown"] = inp.keyEventFn;
          events["keyup"] = inp.keyEventFn;        
          events["touchstart"] = events["touchend"] = events["touchmove"] = events["touchcancel"] = inp.touchEventFn;
          events["mousedown"] = events["mouseup"] = events["mousemove"] = inp.mouseEventFn;
          events["focusout"] = inp.focusoutEventFn;
          events["click"] = inp.clickEventFn;
  
          for (var ev in events)
              canvas.addEventListener(ev, events[ev]);
                 
          return true;   
      }

  function _js_inputResetStreams(maxLen,destPtr) {
          var inp = ut._HTML.input;
          inp.mouseStream.length = 0;
          inp.keyStream.length = 0;
          inp.touchStream.length = 0;
      }

   

   

  function _llvm_bswap_i64(l, h) {
      var retl = _llvm_bswap_i32(h)>>>0;
      var reth = _llvm_bswap_i32(l)>>>0;
      return ((setTempRet0(reth),retl)|0);
    }

  function _llvm_trap() {
      abort('trap!');
    }

  
  function _emscripten_memcpy_big(dest, src, num) {
      HEAPU8.set(HEAPU8.subarray(src, src+num), dest);
    }
  
   

   

   

  
  function _emscripten_get_heap_size() {
      return TOTAL_MEMORY;
    }
  
  function _emscripten_resize_heap(requestedSize) {
      return false; // malloc will report failure
    } 
if (typeof dateNow !== 'undefined') {
    _emscripten_get_now = dateNow;
  } else if (typeof performance === 'object' && performance && typeof performance['now'] === 'function') {
    _emscripten_get_now = function() { return performance['now'](); };
  } else {
    _emscripten_get_now = Date.now;
  };
var GLctx; GL.init();
for (var i = 0; i < 32; i++) __tempFixedLengthArray.push(new Array(i));;
Fetch.staticInit();;
var ut;;
// ASM_LIBRARY EXTERN PRIMITIVES: Int8Array,Int32Array

var debug_table_fi = [0,'_Enumerator_get_Current_m5ABD17D4EF40AADB0BC0A04C080F2B4CA4C5AAF2_AdjustorThunk','_Enumerator_get_Current_m651DA5B0FADBD2F67BC0771E89AC163EFB280FAF_AdjustorThunk','_Enumerator_get_Current_m3A8E1233979BA80E15AAFEA74AAF42EB983D77ED_AdjustorThunk','_Enumerator_get_Current_m337A8CCDB88346A335818265995DFC4F4B537E58_AdjustorThunk','_Enumerator_get_Current_mF6185F281AD0A44F96F98BA87E59D7234EBFA15D_AdjustorThunk',0,0];
var debug_table_i = [0,'_RunLoopImpl_ManagedRAFCallback_mF925FE255AA713688A997187358E933BB3C01E3E','_ReversePInvokeWrapper_RunLoopImpl_ManagedRAFCallback_mF925FE255AA713688A997187358E933BB3C01E3E','_GC_never_stop_func','_GC_timeout_stop_func','_emscripten_glCreateProgram','_emscripten_glGetError',0];
var debug_table_idi = [0,'__ZL4tickdPv'];
var debug_table_ii = [0,'_ValueType_GetHashCode_m1B6B51019DE497F4593F85245565A083D8EC5ECC','_Object_ToString_m2F8E1D9C39999F582E7E3FB8C25BDE64CF5D3FB1','_Object_GetHashCode_m0124B0EA741D727FB7F634BE12BD76B09AB61539','_String_GetHashCode_m92B35EDBE7FDC54BFC0D7189F66AB9BEB8A448D6','_String_ToString_mB0D08BCA549F28AB02BF4172734FA03CEE10BDEF','_Boolean_ToString_m21623BAD041ACEB9C6D1D518CEC0557836BFEB3E_AdjustorThunk','_Int32_GetHashCode_mBA6D17ACDEA463332E5BEE01CFBF7655565F68AB_AdjustorThunk','_Int32_ToString_mD4F198CBC9F482089B366CC486A2AE940001E541_AdjustorThunk','_Char_ToString_mB436886BB2D2CAA232BD6EDFDEBC80F1D8167793_AdjustorThunk','_Double_ToString_mCF8636E87D2E7380DC9D87F9D65814787A1A9641_AdjustorThunk','_UInt32_GetHashCode_mEE25741A74BF35F40D9ECE923222F0F9154E55C2_AdjustorThunk','_UInt32_ToString_mC9C8805EFE6AD403867D30A7364F053E1502908A_AdjustorThunk','_UInt64_GetHashCode_m04995EC62B0C691D5E18267BA59AA04C2C274430_AdjustorThunk','_UInt64_ToString_mC13424681BDC2B62B25ED921557409A1050D00E2_AdjustorThunk','_Type_ToString_m40E1B66CB7DE4E17EE80ED913F8B3BF2243D45F1','_Guid_GetHashCode_m170444FA149D326105F600B729382AF93F2B6CA8_AdjustorThunk','_Guid_ToString_mD0E5721450AAD1387B5E499100EDF9BB9C693E0B_AdjustorThunk','_IntPtr_GetHashCode_m7CFD7A67C9A53C3426144DA5598C2EA98F835C23_AdjustorThunk','_IntPtr_ToString_mA58A6598C07EBC1767491778D67AAB380087F0CE_AdjustorThunk','_Enum_GetHashCode_mC40D81C4EE4A29E14298917C31AAE528484F40BE','_SByte_GetHashCode_m718B3B67E8F7981E0ED0FA754EAB2B5F4A8CFB02_AdjustorThunk','_SByte_ToString_m1206C37C461F0FCB10FB91C43D8DB91D0C66ADAE_AdjustorThunk','_Byte_GetHashCode_mA72B81DA9F4F199178D47432C6603CCD085D91A1_AdjustorThunk','_Byte_ToString_m763404424D28D2AEBAF7FAA8E8F43C3D43E42168_AdjustorThunk','_Int16_GetHashCode_mF465E7A7507982C0E10B76B1939D5D41263DD915_AdjustorThunk','_Int16_ToString_m7597E80D8DB820851DAFD6B43576038BF1E7AC54_AdjustorThunk','_UInt16_GetHashCode_mE8455222B763099240A09D3FD4EE53E29D3CFE41_AdjustorThunk','_UInt16_ToString_m04992F7C6340EB29110C3B2D3F164171D8F284F2_AdjustorThunk','_Int64_GetHashCode_m20E61A76FF573C96FE099C614286B4CDB6BEDDDC_AdjustorThunk','_Int64_ToString_m4FDD791C91585CC95610C5EA5FCCE3AD876BFEB1_AdjustorThunk','_UIntPtr_GetHashCode_m559E8D42D8CF37625EE6D0C3C26B951861EE67E7_AdjustorThunk','_UIntPtr_ToString_m81189D03BA57F753DEEE60CB9D7DE8F4829EEA65_AdjustorThunk','_Single_ToString_mF63119C000259A5CA0471466393D5F5940748EC4_AdjustorThunk','_bool3_GetHashCode_m10E20CB0A27BA386FB3968D8933FF4D9A5340ED7_AdjustorThunk','_bool3_ToString_m823DE53F353DDC296F35BC27CD7EB580C36BB44B_AdjustorThunk','_bool4_GetHashCode_m937BB6FB351DAEFF64CC8B03E9A45F52EECD778A_AdjustorThunk','_bool4_ToString_m1EFC2F937BFB00EA4A7198CF458DD230CC3CEDAA_AdjustorThunk','_float2_GetHashCode_mA948401C52CE935D4AABCC4B0455B14C6DFFCD16_AdjustorThunk','_float2_ToString_m481DE2F7B756D63F85C5093E6DDB16AD5F179941_AdjustorThunk','_float4_GetHashCode_m25D29A72C5E2C21EE21B4940E9825113EA06CFAB_AdjustorThunk','_float4_ToString_m4B13F8534AC224BDFDB905FE309BC94D4A439C20_AdjustorThunk','_float3_GetHashCode_mC6CE65E980EC31CF3E63A0B83F056036C87498EC_AdjustorThunk','_float3_ToString_mFD939AC9FF050E0B5B8057F2D4CD64414A3286B3_AdjustorThunk','_float3x3_GetHashCode_m65A70424340A807965D04BC5104E0723509392C2_AdjustorThunk','_float3x3_ToString_m9B4217D00C44574E76BBCD01DD4CC02C90133684_AdjustorThunk','_uint3_GetHashCode_mC5C0B806919339B0F1E061BF04A4682943820A70_AdjustorThunk','_uint3_ToString_m17D60A96B38038168016152EAA429A08F26A5112_AdjustorThunk','_float4x4_GetHashCode_m41EA5B94472BCBCF17AFBAAF4E73536AA0CC8352_AdjustorThunk','_float4x4_ToString_mC1AE444284D042813DFFFEA72196C651C8741EBC_AdjustorThunk','_uint4_GetHashCode_m0239AEED2EE7540408472027E6534DAE58D016A8_AdjustorThunk','_uint4_ToString_m520C4C7062B544A4B8BB3C85357459B60B2A002B_AdjustorThunk','_uint2_GetHashCode_m64224B108E7424EDDF94F6113D2A058F64F916D9_AdjustorThunk','_uint2_ToString_mC62FCF92B92133B0812E05044B5937B54D1F6C29_AdjustorThunk','_quaternion_GetHashCode_m53775A9F474E2E5EA3311EAC10B54A3F0BACFDDD_AdjustorThunk','_quaternion_ToString_m7E0B020C681C1A89561CF0204D5959557A5B15F2_AdjustorThunk','_NativeString512_GetHashCode_m87C2382927D6F6DC38B9ADA5A73D883C3C998DC6_AdjustorThunk','_NativeString512_ToString_m7410A5AF5412A5C9EB58AE5FC722320698CC9C00_AdjustorThunk','_EntityQueryBuilder_GetHashCode_mB055AB1BF3D95524DF70793120D07E95E09CDBD3_AdjustorThunk','_AABB_ToString_mF99D24B9478C79AEEFD9CA4281643665AA831893_AdjustorThunk','_ComponentType_GetHashCode_mAA4F2ECFF4A9D241BE8D1F246E8D96750F3C9F86_AdjustorThunk','_ComponentType_ToString_m592DDA2FC9006F7BE2FAE8ADA48A4005B3B188DD_AdjustorThunk','_NativeArray_1_GetHashCode_m0DB13C0C977BFB9108F3EEE50324032BA51DF347_AdjustorThunk','_NativeArray_1_System_Collections_Generic_IEnumerableU3CTU3E_GetEnumerator_m9602E0C9DC76E6CC9BC1A6E49B5E7AE5A9831662_AdjustorThunk','_NativeArray_1_GetHashCode_mC76FBB24CD1273D78281A7AA427C3BCCB50E04F4_AdjustorThunk','_NativeArray_1_System_Collections_Generic_IEnumerableU3CTU3E_GetEnumerator_mB72D19668A139C1F44C39365E63FEE70E1286D40_AdjustorThunk','_Entity_GetHashCode_mCD1B382965923B4D8F9D5F8D3487567046E4421B_AdjustorThunk','_Entity_ToString_mD13D1E96A001C26F7B67E5A9EE4CDA2583C8395E_AdjustorThunk','_Scene_GetHashCode_m5E6729A8B6DB615771A604CE9FF09EDD44A909E6_AdjustorThunk','_SceneGuid_GetHashCode_m948EDA30482D4DB87F134CB308708CAEA3E4406C_AdjustorThunk','_World_ToString_mADB17B409AF3FFB43A4371D353B89FBD49507B48','_AsyncOp_ToString_mC51C841EF91AB2756867CF0FBD7292C3479FC037_AdjustorThunk','_EntityGuid_GetHashCode_mEF4B9EB71BD66A885943D0A0F5F30E6C65664F92_AdjustorThunk','_EntityGuid_ToString_m1621A722F1F0EC56D449EADCF0096C16E957D18A_AdjustorThunk','_SceneReference_GetHashCode_mC88DAA73E134CDA559B2D8FC255886405619F1F2_AdjustorThunk','_SceneTag_GetHashCode_m4A71390201A1FB19A53E17880D8AF679BD5AB9A5_AdjustorThunk','_SceneTag_ToString_m39DF9A31846A9D97D4879B8BB98A7EB56CC82C67_AdjustorThunk','_SceneSection_GetHashCode_m56EF3A1C2B91DAEF5960F137F2E34490E632F25C_AdjustorThunk','_BuildGroup_GetHashCode_mA12C67D00499BADABA775C0F141C181726A9F39D_AdjustorThunk','_HTMLWindowSystem_GetPlatformWindowHandle_mCBF33C0F67E020CC84427EF54153BF4FC4ECDFCB','_EntityArchetype_GetHashCode_mA1006937A388D62CD9C4DCC150591B0054775D2A_AdjustorThunk','_ComponentTypeInArchetype_GetHashCode_m60FF085A6DAE0D57C5AE8754D5F3150A50824AC5_AdjustorThunk','_ComponentTypeInArchetype_ToString_m62029984A20006D13CE76BCD8E713592DCE5736D_AdjustorThunk','_ArchetypeChunk_GetHashCode_mA09F0D726007722DCBD42C8953CFFC812FDCD4CD_AdjustorThunk','_BlobAssetPtr_GetHashCode_mEC1FA28CD57BA4C429EF19048ADD27E515EE44C1_AdjustorThunk','_List_1_System_Collections_Generic_IEnumerableU3CTU3E_GetEnumerator_m2CE492C839356DF44518859856CE3BE184F60836','_List_1_System_Collections_Generic_IEnumerableU3CTU3E_GetEnumerator_m700AE89140EA61779E627C74BBF49BB2F8777D06','_List_1_System_Collections_Generic_IEnumerableU3CTU3E_GetEnumerator_m611B041169CB7751903D3E64651D435317C15F0F','_NativeArray_1_GetHashCode_mFEB349DE9C7266D55C8BA36C54A298A762DF9620_AdjustorThunk','_NativeArray_1_System_Collections_Generic_IEnumerableU3CTU3E_GetEnumerator_m5B36182E83DF439797AA044CBE7C204682344C78_AdjustorThunk','_NativeArray_1_GetHashCode_mFD890898CF9235360D31A7278664D98423B063FD_AdjustorThunk','_NativeArray_1_System_Collections_Generic_IEnumerableU3CTU3E_GetEnumerator_mD243469954012C4FE03FBF86E0BBBD0F78AB2601_AdjustorThunk','_NativeList_1_System_Collections_Generic_IEnumerableU3CTU3E_GetEnumerator_mCB0097E9A842E832E308A620566F46124CABC809_AdjustorThunk','_Hash128_GetHashCode_mD7F8986BC81FC06E2F5FF3592E978DD7706DF58B_AdjustorThunk','_Hash128_ToString_m320D31CB2D976B1B82831D17330FE957E87A344E_AdjustorThunk','_List_1_System_Collections_Generic_IEnumerableU3CTU3E_GetEnumerator_m00BF019A7F79AD73545DE4C826D2D409B287221C','_NativeArray_1_GetHashCode_m4966C5CCD58C3CA0EEAF30FCCE09FB9CF2203A37_AdjustorThunk','_NativeArray_1_System_Collections_Generic_IEnumerableU3CTU3E_GetEnumerator_mCA824E31A32B692EBBB01FF6E6BDEDB287D943FC_AdjustorThunk','_List_1_System_Collections_Generic_IEnumerableU3CTU3E_GetEnumerator_m8FE16AD757A9286225FA1B40A38A993F27EAB8C8','_List_1_System_Collections_Generic_IEnumerableU3CTU3E_GetEnumerator_mB1E1BD875D9EB349F4925DEDE584079492B710B8','_List_1_System_Collections_Generic_IEnumerableU3CTU3E_GetEnumerator_m9E0F8FF75681BAD09D6D026FC11B4853C86E6658','_Color_GetHashCode_mA50245CD9DE9C30C9D59CD665E6EE38616E4A8D9_AdjustorThunk','_RunLoopDelegate_Invoke_mB498A417DD5ABD7B53FD64D45953F34DEA48E173','_Enumerator_get_Current_mC6ABC79D914E30843E5281248A7B59B3799661CB_AdjustorThunk','_Enumerator_MoveNext_mCF29112702297DA4897A92513CDB1180B66EB43A_AdjustorThunk','_Enumerator_get_Current_m1ECEC59809D0B9EEEC4D7DE98B3A6B057BB6D6F0_AdjustorThunk','_Enumerator_MoveNext_mB496DF87EB078B9069267F641D50CA97CAE09461_AdjustorThunk','_Enumerator_get_Current_m6614170FE1171F7E1D490775C5F219A0B428EC68_AdjustorThunk','_Enumerator_MoveNext_mD114CEB68F7A60A181D3959982B54AEC59F63160_AdjustorThunk','_Enumerator_get_Current_m75695AC77D9CDB17A58C9BD84287F87B9045D678_AdjustorThunk','_Enumerator_MoveNext_mBC614844377085D8D66A91E7301A02C4357D9D2E_AdjustorThunk','_Enumerator_MoveNext_m802D6F6C750B08E3061672D81E158203290842DA_AdjustorThunk','_Enumerator_MoveNext_m4A5C1777E3A4E491D58EE9B34B25AEA40ECEC74A_AdjustorThunk','_Enumerator_get_Current_mD43EF163773453F38EC03CACD91C76CE087B17F1_AdjustorThunk','_Enumerator_MoveNext_mEC2C2490AC554887909C9B6E50EFBD51759FB66F_AdjustorThunk','_Enumerator_get_Current_m972B9249BB3FA7D6889F7CB294789D3F665DCEB2_AdjustorThunk','_Enumerator_MoveNext_mB6D8761D0655224E293B9D462E6611F227AB2A6B_AdjustorThunk','_NativeArray_1_GetHashCode_m0D1D5019BF66CAD007B84064F3CDB2D69C0888F3_AdjustorThunk','_NativeArray_1_System_Collections_Generic_IEnumerableU3CTU3E_GetEnumerator_m44B52A68658F5B8730D27424E0C71AE9BB8F9025_AdjustorThunk','_Enumerator_get_Current_mAD1D6A047F7E0A08CC02176ADD6F19FB72A60360_AdjustorThunk','_Enumerator_MoveNext_m9A2AE49D3675A14AAD78F1534BAB812D40E60003_AdjustorThunk','_NativeArray_1_GetHashCode_m10806976ACA31A415C7F48618F8101C1B97BFED2_AdjustorThunk','_NativeArray_1_System_Collections_Generic_IEnumerableU3CTU3E_GetEnumerator_m487B7ED111AF1BC767A3D937F5C74C4C707BE95A_AdjustorThunk','_Enumerator_get_Current_mDF8C7CB079005C8869B49AB631601F72924E5028_AdjustorThunk','_Enumerator_MoveNext_m024EAED6AF42B7883E66FF40591F74C9A60FBB08_AdjustorThunk','_Enumerator_get_Current_m6B3A6191FB7F38B9F4423766BAE0CA1A1F2B6FA7_AdjustorThunk','_Enumerator_MoveNext_m00E75A617196E4990F84C085DC6FC3006B730062_AdjustorThunk','_NativeArray_1_GetHashCode_mE9F0C432A12C17DCB7542670BCE97AA73F29181C_AdjustorThunk','_NativeArray_1_System_Collections_Generic_IEnumerableU3CTU3E_GetEnumerator_mE941938C11659FA301F31D8C3D733340462E7B32_AdjustorThunk','_Enumerator_MoveNext_m7BBFD970FB8DCCF7500BE762A2F328AA91C3E645_AdjustorThunk','_NativeArray_1_GetHashCode_m27C3DECFC4B1BD6E506B6810B4DF050C360C8EB9_AdjustorThunk','_NativeArray_1_System_Collections_Generic_IEnumerableU3CTU3E_GetEnumerator_mD3594D7AF499958B55E3157B7ABA8911B0F3E097_AdjustorThunk','_Enumerator_MoveNext_m9EBB1020E59CE6531D6BAE5776D64F01E73592FF_AdjustorThunk','_NativeArray_1_GetHashCode_m046207D9884C4DCE9AC88C8C62F2C1CEC4E73093_AdjustorThunk','_NativeArray_1_System_Collections_Generic_IEnumerableU3CTU3E_GetEnumerator_mB2F99E93B69580E4D8ECA0352148479C34DC5926_AdjustorThunk','_Enumerator_MoveNext_m0D19CA506EB45D7C3E49EBF5D288B4D3DC55E716_AdjustorThunk','_NativeArray_1_GetHashCode_mB0F4D854505A5BA70C063CE2CCD0AEC56CF550FC_AdjustorThunk','_NativeArray_1_System_Collections_Generic_IEnumerableU3CTU3E_GetEnumerator_mBC32127E0E001BD0441EC1C83F43511851D45B8B_AdjustorThunk','_Enumerator_MoveNext_m2DBDBC32779DD1D8C32C11C0CC18C85C1ECB4BC3_AdjustorThunk','_NativeArray_1_GetHashCode_m2BEF0CC741B083134C1EE8B2608933F556543264_AdjustorThunk','_NativeArray_1_System_Collections_Generic_IEnumerableU3CTU3E_GetEnumerator_m96687405C2E5C061F7879D3C3AB87E544A920E65_AdjustorThunk','_Enumerator_MoveNext_m60A400CD7C541F0416092427C159CE3CC2AF381E_AdjustorThunk','_NativeArray_1_GetHashCode_m7E5E1E85CF44628EF58A1F44A6F20624C01A3B50_AdjustorThunk','_NativeArray_1_System_Collections_Generic_IEnumerableU3CTU3E_GetEnumerator_m463997A72A578AF7531A3AC166BCA95D8E86C347_AdjustorThunk','_Enumerator_MoveNext_mB7CB39B32F083499AA3FAB67CEEDCC779EFA1A6E_AdjustorThunk','_NativeArray_1_GetHashCode_m927BB00302CC26EA057E7DE022F142CD14D44B36_AdjustorThunk','_NativeArray_1_System_Collections_Generic_IEnumerableU3CTU3E_GetEnumerator_m2B0E6CE906CACC89BA4131E16A2F7EC70F76288E_AdjustorThunk','_Enumerator_MoveNext_m9549A36A0A07C8EB7C118E97617025CDCECF945F_AdjustorThunk','_NativeArray_1_GetHashCode_m28C64CAC0E19E2B9B64F0042AC9B8DE3C481A56A_AdjustorThunk','_NativeArray_1_System_Collections_Generic_IEnumerableU3CTU3E_GetEnumerator_m004D3F3984F64E60DF82CCBB3E25D4DEFDED2171_AdjustorThunk','_Enumerator_MoveNext_m46008EA5EDCB84133C9475C1075908D263C5714E_AdjustorThunk','_NativeArray_1_GetHashCode_mC8BA5ABB1DFFFF6A52549945C4EE29751CB94A6F_AdjustorThunk','_NativeArray_1_System_Collections_Generic_IEnumerableU3CTU3E_GetEnumerator_m1D5F0EA39D2D3F0E2EF48E5E7E35B5B61D2149DC_AdjustorThunk','_Enumerator_MoveNext_mF343309A309E7CE3019D245A1DACAB32D839BB9F_AdjustorThunk','_NativeArray_1_GetHashCode_m42A0A06BD74F88DF129FCD20042CE5DFE3474D6A_AdjustorThunk','_NativeArray_1_System_Collections_Generic_IEnumerableU3CTU3E_GetEnumerator_mEC64E1A30BEAC47BE901AFF13B52BE466C473CB0_AdjustorThunk','_Enumerator_MoveNext_m716140FF4ED64D61ADD9EA52713FE83BB259B510_AdjustorThunk','_NativeArray_1_GetHashCode_m775D29C673D5E6D554A6A80603A7DCD91C0E4B8A_AdjustorThunk','_NativeArray_1_System_Collections_Generic_IEnumerableU3CTU3E_GetEnumerator_m7BCF1C1F12F89E678FCC4222D058AE7A02D652CC_AdjustorThunk','_Enumerator_MoveNext_mC9643C167C735FED6D4A1770911D516AC635254E_AdjustorThunk','_NativeArray_1_GetHashCode_m77581E6BC46D083C6F2F5972550CA77423EA63E3_AdjustorThunk','_NativeArray_1_System_Collections_Generic_IEnumerableU3CTU3E_GetEnumerator_mBD6A83BE4356EC47082C2F1D2EA7A5337E34B94B_AdjustorThunk','_Enumerator_MoveNext_m6E25116189C7E2235EFE372585D7D6F0E15F3F59_AdjustorThunk','_NativeArray_1_GetHashCode_mF730CC227FEEE3C1C16EE79D7B0496405A9AB456_AdjustorThunk','_NativeArray_1_System_Collections_Generic_IEnumerableU3CTU3E_GetEnumerator_mCC14AD24FD31910497B8E9549DE86EEC2FB20EDC_AdjustorThunk','_Enumerator_MoveNext_mA47AD6EE937803D2A03966319D57CC8663DDB4B7_AdjustorThunk','_NativeArray_1_GetHashCode_m1C77DFB3513EE2DE2F57BD881DF91B7876876413_AdjustorThunk','_NativeArray_1_System_Collections_Generic_IEnumerableU3CTU3E_GetEnumerator_mC521CA55F229DA50D41F0881E925AA96D33E03F8_AdjustorThunk','_Enumerator_MoveNext_mBF717E9C5A38C7F5F3585D4C1403B19300B7960C_AdjustorThunk','_Enumerator_MoveNext_m5F8619203D4872B1E0C80AED3E700B78D014C8D2_AdjustorThunk','_NativeArray_1_GetHashCode_mC0F0669424822ED96181D81B1B1DE6C3D5C519D3_AdjustorThunk','_NativeArray_1_System_Collections_Generic_IEnumerableU3CTU3E_GetEnumerator_m7734C6F9EFB677339F3950E734C9C51C91EA12ED_AdjustorThunk','_Enumerator_MoveNext_m46A8DA06205EA5FBE9C50544CC4B18A701BD7EAC_AdjustorThunk','_NativeArray_1_GetHashCode_m28EBA687533E6A283F82817C099FDCA72B223B18_AdjustorThunk','_NativeArray_1_System_Collections_Generic_IEnumerableU3CTU3E_GetEnumerator_m681502D8F769F1F62DF6CC3C5CC1B15DD96DD2A5_AdjustorThunk','_Enumerator_MoveNext_m527BD14C255F63FA44086AC1C13F19E7AD179217_AdjustorThunk','_Enumerator_get_Current_m2B47245DB3003B76DF4958188BE5CDD2463B4738_AdjustorThunk','_Enumerator_MoveNext_m4256FBE26BC283A0E66E428A7F51CD155025FBFE_AdjustorThunk','_Enumerator_MoveNext_m478C96CD7A31BBAE599B699F1332C3C6A4168ED4_AdjustorThunk','_NativeArray_1_GetHashCode_m6C126C524D290AD5CEF02957ECEC003D66D6A965_AdjustorThunk','_NativeArray_1_System_Collections_Generic_IEnumerableU3CTU3E_GetEnumerator_m5DB74A9A2D0001EAA346B834DD36A5F7E3A9F415_AdjustorThunk','_Enumerator_MoveNext_m3E36FA7F1CF04BF62D2FBA0071178BF0AA75D953_AdjustorThunk','_NativeArray_1_GetHashCode_mE5A1D77C13E970391EDC12DDA1D67ADB2423EEC5_AdjustorThunk','_NativeArray_1_System_Collections_Generic_IEnumerableU3CTU3E_GetEnumerator_mA68B8ACD22836B0DCB481FBD2C3C9D69AC6825C3_AdjustorThunk','_Enumerator_MoveNext_m5E5023FBA26AD5BE482B66445F7A33D4AE8B34BE_AdjustorThunk','_NativeArray_1_GetHashCode_m1A58E3EC7DF72389A8846B623C7ED3F5FD1E83F1_AdjustorThunk','_NativeArray_1_System_Collections_Generic_IEnumerableU3CTU3E_GetEnumerator_m6A8FF0D1C6507906CDFD73C884A488BCA665FBED_AdjustorThunk','_Enumerator_MoveNext_mDFC9653D896ADE94D9299F39A28A1702E054C5B8_AdjustorThunk','_NativeArray_1_GetHashCode_m0B5D21EA1441CFD6012053112F49AFE5AC43E066_AdjustorThunk','_NativeArray_1_System_Collections_Generic_IEnumerableU3CTU3E_GetEnumerator_m353EFA9293CCF00DD983C7DDF1167ED6A352E96A_AdjustorThunk','_Enumerator_MoveNext_m479D00B49840C2CB34D76D674CAC6DA65362DAED_AdjustorThunk','_NativeArray_1_GetHashCode_m5AFF2FCEDCD57E6C2E5DDE78A96C482768FA8588_AdjustorThunk','_NativeArray_1_System_Collections_Generic_IEnumerableU3CTU3E_GetEnumerator_m3EEE64FD23DD552E74E39AAD8B2E8D0AF2E0D600_AdjustorThunk','_Enumerator_MoveNext_m1B69B4E8587374D22850861E13B691EF88FCEFE5_AdjustorThunk','_Enumerator_MoveNext_m2139443A58F0B4BEFC29B2E2162876B42346C1FC_AdjustorThunk','_NativeArray_1_GetHashCode_m6ACAE362C6CCE9443BA975C764094ACA191FA358_AdjustorThunk','_NativeArray_1_System_Collections_Generic_IEnumerableU3CTU3E_GetEnumerator_m35EAC12C7134FD8141C01E8FFC74FAF61F928439_AdjustorThunk','_Enumerator_get_Current_m13CED33F34399EC787D79B1C4B9C1A4E9FEF2922_AdjustorThunk','_Enumerator_MoveNext_m3ADF2AD1DB95431386FB247D014486F7AE600C6D_AdjustorThunk','_NativeArray_1_GetHashCode_mF4D73AA637B768524AA7900C22B929DBE637CE26_AdjustorThunk','_NativeArray_1_System_Collections_Generic_IEnumerableU3CTU3E_GetEnumerator_m03B3C3DE93110911F76408A06331F2416AF900DC_AdjustorThunk','_Enumerator_get_Current_m31454E65A97BBB7FF25AE3AAD3643DD99B4398F8_AdjustorThunk','_Enumerator_MoveNext_mFB5137699EB1D9F26746E5867151558AE83E84E1_AdjustorThunk','_NativeArray_1_GetHashCode_m198A88209BA92B61F1E65BBA478FD0AA5ABA172E_AdjustorThunk','_NativeArray_1_System_Collections_Generic_IEnumerableU3CTU3E_GetEnumerator_m6424AAE0BD540228EA103B4F5C092F3AB3371D20_AdjustorThunk','_Enumerator_MoveNext_m95784C62D63E3C6E6EC7296FD0AA715EC135BE61_AdjustorThunk','_NativeArray_1_GetHashCode_mFC6BB1B50E37EDFC6F990250F62CEFABDEEB6DCC_AdjustorThunk','_NativeArray_1_System_Collections_Generic_IEnumerableU3CTU3E_GetEnumerator_m9DDF55EAA4EF5B6851CAAAEB467F3CE0F17CA7A2_AdjustorThunk','_Enumerator_MoveNext_mE7D755A9C770999097F11AE543AC1C171AA1068A_AdjustorThunk','_NativeArray_1_GetHashCode_m0D4DE454C46AF6B29D44ECEF9098A2A0CECFA959_AdjustorThunk','_NativeArray_1_System_Collections_Generic_IEnumerableU3CTU3E_GetEnumerator_m5DF0C982062C972965D52F726B4591680A18389E_AdjustorThunk','_Enumerator_MoveNext_mF76AD13B2F61A40CF9816952DAEDE9D2002C3EA0_AdjustorThunk','_NativeArray_1_GetHashCode_m9C06C67C3050C446C5611FF382A6CA8ABF05C38F_AdjustorThunk','_NativeArray_1_System_Collections_Generic_IEnumerableU3CTU3E_GetEnumerator_m721A5E5E200991BD9FFCC3E135CB0398F91936B8_AdjustorThunk','_Enumerator_get_Current_m662DF0B6737DFF8E789A55EC9B0BF3DBFAC4B4C2_AdjustorThunk','_Enumerator_MoveNext_m795868D6E72DA5CFBB1ABEDC87F7DD8F3FB8A155_AdjustorThunk','_NativeArray_1_GetHashCode_m0034C504DAE536397CBCB1175086A12E8EB329CD_AdjustorThunk','_NativeArray_1_System_Collections_Generic_IEnumerableU3CTU3E_GetEnumerator_mF7B0DFC2FA0789CBC96A3D9859BA6A8610B9E588_AdjustorThunk','_Enumerator_MoveNext_m520E08BE088F67C0334D6E091330489C377ECCB0_AdjustorThunk','_NativeArray_1_GetHashCode_m9142745576EFFBDF02436D21101CAD6CC6E40463_AdjustorThunk','_NativeArray_1_System_Collections_Generic_IEnumerableU3CTU3E_GetEnumerator_m29470A1D434848F28F6019D6C2022CD989967968_AdjustorThunk','_Enumerator_MoveNext_m61D9A389EF8AC75299078DC0B2ED4120ACA8B908_AdjustorThunk','_NativeArray_1_GetHashCode_m6A0C4A60552E87B029CA2C85642AF1BEF5BD5197_AdjustorThunk','_NativeArray_1_System_Collections_Generic_IEnumerableU3CTU3E_GetEnumerator_m1D4E9988DF0976C4CCE48DC614F771C8F8C4986C_AdjustorThunk','_Enumerator_MoveNext_m6ED50098C9C928510A0B94A509BEFE96F92D2633_AdjustorThunk','_NativeArray_1_GetHashCode_m057D0FF269F2D1B97EF2BDDBCB151CD4D4D5C829_AdjustorThunk','_NativeArray_1_System_Collections_Generic_IEnumerableU3CTU3E_GetEnumerator_mA9960AD928747D86BC483094249D19A0969E697B_AdjustorThunk','_Enumerator_MoveNext_m4D62BDE61547BEDE9C8648FF7EB3D477B0E69B07_AdjustorThunk','_NativeArray_1_GetHashCode_mDCA5EEEB3E63CCED3E97F547E689C378803C5AF8_AdjustorThunk','_NativeArray_1_System_Collections_Generic_IEnumerableU3CTU3E_GetEnumerator_m288E30BFBA5765DB5D39FFEE41F66C84E79CBB73_AdjustorThunk','_Enumerator_MoveNext_mDB3C65DCA17109605BDAF618BB6602315550D4A9_AdjustorThunk','_NativeArray_1_GetHashCode_mE7997D719B4F20E17117A1C12B95A428F05BA9A8_AdjustorThunk','_NativeArray_1_System_Collections_Generic_IEnumerableU3CTU3E_GetEnumerator_m860B40B98233B6E0FA4619F9349422C90C9E1A98_AdjustorThunk','_Enumerator_get_Current_m57DD57AC10CA6CE7700EDE23C043B03DB6611CF3_AdjustorThunk','_Enumerator_MoveNext_m6447DBEC025AFD71454408F5B026E4B075282D7E_AdjustorThunk','_NativeArray_1_GetHashCode_mF531A0053C2C889F2697F1B30D802FA55692C937_AdjustorThunk','_NativeArray_1_System_Collections_Generic_IEnumerableU3CTU3E_GetEnumerator_mE537A3D0A7CE4F9F6A32CD7ABB14FA6C37BF3C75_AdjustorThunk','_Enumerator_get_Current_m9D1396BB7E732404C7E8AA214A9BA9A632F60D1E_AdjustorThunk','_Enumerator_MoveNext_m88B50F98F0998F40114FBAF1E77F15F14177F88A_AdjustorThunk','_NativeArray_1_GetHashCode_m3ED44B56BE820B99862642E15141A24604120358_AdjustorThunk','_NativeArray_1_System_Collections_Generic_IEnumerableU3CTU3E_GetEnumerator_m79DEE332EE07B13F34F73DE829E7F8002130255E_AdjustorThunk','_Enumerator_get_Current_m58F8EB07DDBDCB59090155D105993442874B7487_AdjustorThunk','_Enumerator_MoveNext_m831EEB487B20953108235F478969BB1A44B81B5C_AdjustorThunk','_NativeArray_1_GetHashCode_mCD736C3B1CB0E56FFCC130C57DB1FA67AEF0A00E_AdjustorThunk','_NativeArray_1_System_Collections_Generic_IEnumerableU3CTU3E_GetEnumerator_mCE1E1621618447F7D9D270512E9BE717B9340E05_AdjustorThunk','_Enumerator_get_Current_m7EC34EA3F22753CA9A4A2D685E84AAE8CAC78849_AdjustorThunk','_Enumerator_MoveNext_m83BCC29B5F2D449CB0617662B5EA30C5291AD811_AdjustorThunk','_NativeArray_1_GetHashCode_mBE537F4313627BC175C465A183B16A3E1C2A2952_AdjustorThunk','_NativeArray_1_System_Collections_Generic_IEnumerableU3CTU3E_GetEnumerator_mBDC495DB4EBAE957A5845274ADADF24BC3BCA19E_AdjustorThunk','_Enumerator_get_Current_m46F3A84863B4984F8D9FB33F3D3DF409CADDAF30_AdjustorThunk','_Enumerator_MoveNext_m827294D73873ABFCD9637AA3880DD56CD22F0E32_AdjustorThunk','_NativeArray_1_GetHashCode_mF8D9CF50F336B4C0013F36D4B29FE16944E1E10A_AdjustorThunk','_NativeArray_1_System_Collections_Generic_IEnumerableU3CTU3E_GetEnumerator_m0E71E75218E80281D9636B95ADA8BE74FB5A1964_AdjustorThunk','_Enumerator_get_Current_m6E56A1D70E342BF4AE212C6AF784A3DDAFDA6262_AdjustorThunk','_Enumerator_MoveNext_m23A14502E9EBA2E2E038CE603E8B7C3E081608DF_AdjustorThunk','_NativeArray_1_GetHashCode_m1707F2FA7A034BEAD69BA09B4CDEDFC39AED1FCB_AdjustorThunk','_NativeArray_1_System_Collections_Generic_IEnumerableU3CTU3E_GetEnumerator_m37D4437C91110748ACD7D90A48B27D3E8DB8224D_AdjustorThunk','_Enumerator_MoveNext_mBFF6E026D360EE2F9554B45C22B460C2F645EF14_AdjustorThunk','_NativeArray_1_GetHashCode_m9F937E325F84FEDA08503A80BBA96EBEA278837C_AdjustorThunk','_NativeArray_1_System_Collections_Generic_IEnumerableU3CTU3E_GetEnumerator_m21942F1B6127BE4E2698C47145BB82A3EEA7A7F9_AdjustorThunk','_Enumerator_MoveNext_mAE23BBEA93A3CFC9B9D159AEB8FD41713DE211FC_AdjustorThunk','_NativeArray_1_GetHashCode_mDDA5D4BC79A8BF91BFA2933A9AB6FA99715C044B_AdjustorThunk','_NativeArray_1_System_Collections_Generic_IEnumerableU3CTU3E_GetEnumerator_mBABE5A344AD7408345EA45FC75829F71239485B3_AdjustorThunk','_Enumerator_MoveNext_m697C490540EE56340311A3E596D69C72C7B40846_AdjustorThunk','_NativeArray_1_GetHashCode_m0C4339690719DDD6F9F445ADB8B706753499841B_AdjustorThunk','_NativeArray_1_System_Collections_Generic_IEnumerableU3CTU3E_GetEnumerator_mE516D50C6AD0B06F7DF5EE79E32371AB7DB7FA72_AdjustorThunk','_Enumerator_MoveNext_mB21306C2E63F54303FA555C4AFBB378CBB3982B3_AdjustorThunk','_NativeArray_1_GetHashCode_m2D27332537D6C71790B7101F7A579F2738EB5199_AdjustorThunk','_NativeArray_1_System_Collections_Generic_IEnumerableU3CTU3E_GetEnumerator_m6B651861BA2320FC7F3464CC5B3B73C7C072CAF2_AdjustorThunk','_Enumerator_MoveNext_mD2C3DB72BEDE5D5EEE83A8F41C320EB8D14E839C_AdjustorThunk','_NativeArray_1_GetHashCode_mCF629540DEC954537117670E7A2D8530BB5477E6_AdjustorThunk','_NativeArray_1_System_Collections_Generic_IEnumerableU3CTU3E_GetEnumerator_mED8C839219267836F95D1A8B09FFF30C5FC7B074_AdjustorThunk','_Enumerator_MoveNext_m9CA6BFA547770E374EDA26B0F6FAC453033E6137_AdjustorThunk','_NativeArray_1_GetHashCode_mB5D9CF4C9C3660357F45CA35AC99A0725B9A085E_AdjustorThunk','_NativeArray_1_System_Collections_Generic_IEnumerableU3CTU3E_GetEnumerator_mAE3BBFA4A9662507C1094C962436579C88FBD70F_AdjustorThunk','_Enumerator_MoveNext_mFE3D7D0ED8EEFC917563DE4A381D9E443E45107F_AdjustorThunk','_NativeArray_1_GetHashCode_m5415C7F8A24EF3700A729AC117543724924AADF7_AdjustorThunk','_NativeArray_1_System_Collections_Generic_IEnumerableU3CTU3E_GetEnumerator_m5BE17E2A8C56353B35ACFD20944DD006532DFBEE_AdjustorThunk','_Enumerator_MoveNext_mF7977A3DC7E791290E1125BB6552B8097195E53A_AdjustorThunk','_NativeArray_1_GetHashCode_mB030EB8C08E4BE3B5A94C73DBD4548B0E3415F67_AdjustorThunk','_NativeArray_1_System_Collections_Generic_IEnumerableU3CTU3E_GetEnumerator_m1311E0C53A7D29ABBC92CA612E2D82DFD68140E0_AdjustorThunk','_Enumerator_MoveNext_m47E28F8562C29FE0044303479332E0C8F2DB4616_AdjustorThunk','_NativeArray_1_GetHashCode_m909C25534AF8E0FB4735057DF513EA17721E3DA3_AdjustorThunk','_NativeArray_1_System_Collections_Generic_IEnumerableU3CTU3E_GetEnumerator_mC33800F346B477093A6EDC5D4AFC4431F5C9D754_AdjustorThunk','_Enumerator_MoveNext_m068D544063206383C2D32F41E8EEB107FDE4332A_AdjustorThunk','_NativeArray_1_GetHashCode_mBE4E6E4C056B6A63958EA150B12B6866B0E5ED86_AdjustorThunk','_NativeArray_1_System_Collections_Generic_IEnumerableU3CTU3E_GetEnumerator_mD997C09BB6B62BEEB1305F5306E16B2BC3B9F4CD_AdjustorThunk','_Enumerator_MoveNext_mE1F2638FF47B3825ECC00ECE3B4FC8652E10F69E_AdjustorThunk','_NativeArray_1_GetHashCode_m08171CCB397A0E029E7B4F01515786272B38E6AB_AdjustorThunk','_NativeArray_1_System_Collections_Generic_IEnumerableU3CTU3E_GetEnumerator_m7AC79BEA9E1EA359F1F53BB4881F9B81B9CD3048_AdjustorThunk','_Enumerator_MoveNext_mA6C10E5DA299835601A98A266EFA7E3EAC1CF4BD_AdjustorThunk','_NativeArray_1_GetHashCode_m1F6800E8F7E2B650805D20B8AC93338E396F10F9_AdjustorThunk','_NativeArray_1_System_Collections_Generic_IEnumerableU3CTU3E_GetEnumerator_m34DDDBC63C58F33600BD1D8E66CD5B9E742FD1E9_AdjustorThunk','_Enumerator_MoveNext_mC5352E1656E9647E5DC75FAC572AABE7DF725A44_AdjustorThunk','_NativeArray_1_GetHashCode_m52513DD9F408CE2CDADED643872F93F56A59A1AC_AdjustorThunk','_NativeArray_1_System_Collections_Generic_IEnumerableU3CTU3E_GetEnumerator_m8240FB8E286A5BCAB1AD1B00E0A6654F72A3CFB1_AdjustorThunk','_Enumerator_MoveNext_m504D831A190C3FDE4FAA5CE50622F05C5ACAABB5_AdjustorThunk','_NativeArray_1_GetHashCode_m967F032AF87E3DAAE3D31D0C2FB4D5C274A704E2_AdjustorThunk','_NativeArray_1_System_Collections_Generic_IEnumerableU3CTU3E_GetEnumerator_m88D819512A23462B4D8881BB6256334B6FF3009D_AdjustorThunk','_Enumerator_MoveNext_m41CBEC93BF4229AD610DF5DE7919162A1AE7A371_AdjustorThunk','_NativeArray_1_GetHashCode_m53E39F4777CD14E5AEE86590C9D722C8C0804F0D_AdjustorThunk','_NativeArray_1_System_Collections_Generic_IEnumerableU3CTU3E_GetEnumerator_m49F7301FC81E0732B564FA4FB8C915DB656F0ED0_AdjustorThunk','_Enumerator_get_Current_m52317E2BC62F118C9D4B913112A929A6040D91DD_AdjustorThunk','_Enumerator_MoveNext_m62AE692787E8F5A07661A55951ECBEE2F1733764_AdjustorThunk','_NativeArray_1_GetHashCode_mDEA77C70F60F556DFFB0398DE095CA4CCCB8573C_AdjustorThunk','_NativeArray_1_System_Collections_Generic_IEnumerableU3CTU3E_GetEnumerator_mAB3BC4F5B043A51F812A04E336B0F56861C85828_AdjustorThunk','_Enumerator_MoveNext_m731A44C000D1FCA90308DFBAE86A1F81C75B38F8_AdjustorThunk','_NativeArray_1_GetHashCode_m7A80E2BD16B6BBCA9D984A3B134E101DF2A00CE2_AdjustorThunk','_NativeArray_1_System_Collections_Generic_IEnumerableU3CTU3E_GetEnumerator_m99D0124DEB036BF984F6612ECB4BBB7FAE3227A9_AdjustorThunk','_Enumerator_get_Current_m28AA89F7C2B07BAAD63EF46DCF6E8A720189508A_AdjustorThunk','_Enumerator_MoveNext_m331DAD0FAFACCB84108C5C28A933DBBC0ED65667_AdjustorThunk','_NativeArray_1_GetHashCode_m646215019A26FF1CB4263E0F63F9BED206E3AAB9_AdjustorThunk','_NativeArray_1_System_Collections_Generic_IEnumerableU3CTU3E_GetEnumerator_m81B364D3303AD71184C11985A2FD6C51240D82E8_AdjustorThunk','_Enumerator_MoveNext_m4DC3D5C87A455B4616C92403A4E0565A096481F8_AdjustorThunk','_NativeArray_1_GetHashCode_mDED3C383D8DD0BD78686FC88CD14C3FDB400A07C_AdjustorThunk','_NativeArray_1_System_Collections_Generic_IEnumerableU3CTU3E_GetEnumerator_m11770A1BAB96F850766EAB40314FA9A8A7D0687D_AdjustorThunk','_Enumerator_MoveNext_m8E9D3D556EDAEB3BCA20934B10B9CBBABED46848_AdjustorThunk','_NativeArray_1_GetHashCode_mCC2061D19D934E096417BB6EFB5DB62755B2802D_AdjustorThunk','_NativeArray_1_System_Collections_Generic_IEnumerableU3CTU3E_GetEnumerator_m9655400E16E83BBD580FB1895970DBB89F61A137_AdjustorThunk','_Enumerator_MoveNext_mFDCFC7AB29D691493C863FABDAE71A9EAB0C801B_AdjustorThunk','_NativeArray_1_GetHashCode_m5A8D1E4599E6395293C8C5A703C6CA172B4BC2B1_AdjustorThunk','_NativeArray_1_System_Collections_Generic_IEnumerableU3CTU3E_GetEnumerator_m2E9FDCE31991F73D8CF6EE9EDE05A256E8D22F67_AdjustorThunk','_Enumerator_MoveNext_mC77CF72C1DB5562E75D022FFB0EC32BAF9A5C9EF_AdjustorThunk','_NativeArray_1_GetHashCode_m1C2AFFBACEBAC187236025930C9071401D71C58A_AdjustorThunk','_NativeArray_1_System_Collections_Generic_IEnumerableU3CTU3E_GetEnumerator_mC342641AFB1F788102E466CFC1E9B767D3E24C7F_AdjustorThunk','_Enumerator_MoveNext_mAEE41B121E4499EC5BF38D496532A8A1A6FA4469_AdjustorThunk','_NativeArray_1_GetHashCode_m965C4641D1CB7809B0E78412DEB961B4B110455A_AdjustorThunk','_NativeArray_1_System_Collections_Generic_IEnumerableU3CTU3E_GetEnumerator_m7867C2B8FE344DD522319D4F4ED8BC2B3080763C_AdjustorThunk','_Enumerator_MoveNext_m2D125A6979A6F466DB540CF5F8DCF1086B334DD1_AdjustorThunk','_NativeArray_1_GetHashCode_m70EA13C211DDE4030525DD74AC2F586076125C5B_AdjustorThunk','_NativeArray_1_System_Collections_Generic_IEnumerableU3CTU3E_GetEnumerator_mF7A252DFA4321D1ACFCB89ECB4B99B6A2048A655_AdjustorThunk','_Enumerator_MoveNext_m90B65817F19BEC2FA1CEA8C367EEEAC471CCC6BE_AdjustorThunk','_NativeArray_1_GetHashCode_mAE4CBE4FFB8FC7B886587F19424A27E022C5123B_AdjustorThunk','_NativeArray_1_System_Collections_Generic_IEnumerableU3CTU3E_GetEnumerator_mCE59DA32D424E439BF1379131D0B489A82E0EC7B_AdjustorThunk','_Enumerator_MoveNext_m25407EC4818BDB26661B89E44EC520BCB92383E5_AdjustorThunk','_NativeArray_1_GetHashCode_m91E3B9A3631C724F119588021114313956FF64D8_AdjustorThunk','_NativeArray_1_System_Collections_Generic_IEnumerableU3CTU3E_GetEnumerator_m52F42A301B057C9463D4DD51CF5A613A447CED2F_AdjustorThunk','_Enumerator_MoveNext_mDBFB6094B5FAB259F4A08034823B71B823B98F60_AdjustorThunk','_NativeArray_1_GetHashCode_m1980D96C948D649CF048769BC91078806D7F1952_AdjustorThunk','_NativeArray_1_System_Collections_Generic_IEnumerableU3CTU3E_GetEnumerator_m3E67D5B188282D8913739F458315B6ED91BEDA02_AdjustorThunk','_Enumerator_MoveNext_mAC0F441A3C56468EEDA2D4FFE61E805F7721BC55_AdjustorThunk','_NativeArray_1_GetHashCode_mF8D5F414E757FA2C2DB50DF91F93FEBA0624251B_AdjustorThunk','_NativeArray_1_System_Collections_Generic_IEnumerableU3CTU3E_GetEnumerator_m8B7AD6B1B3F37F2FE665282DFAF69FE8AF891C65_AdjustorThunk','_Enumerator_MoveNext_m2A930399F53D888B078714E1F847A797AECE929F_AdjustorThunk','_NativeArray_1_GetHashCode_m1864F28E54144FBFE208844D3AA37AD72F5D1B7A_AdjustorThunk','_NativeArray_1_System_Collections_Generic_IEnumerableU3CTU3E_GetEnumerator_m11632C3AE38CDBE3588B4DCEFE7F41A6E96C2F38_AdjustorThunk','_Enumerator_MoveNext_mBFC7142744AF5D62505BD2C395AC57495AA7C2EC_AdjustorThunk','_NativeArray_1_GetHashCode_mD6358B9BB31775203640FC2E24DE50DE9BE91444_AdjustorThunk','_NativeArray_1_System_Collections_Generic_IEnumerableU3CTU3E_GetEnumerator_m530BEF0E514065B1C1A89A7C9764B26909196E00_AdjustorThunk','_Enumerator_MoveNext_m6AB4BD52F325959D7E799FB3C0596D6C1FBB610C_AdjustorThunk','_NativeArray_1_GetHashCode_m95FE1AE9C890E875852854A5E5BB643B8B60B4FC_AdjustorThunk','_NativeArray_1_System_Collections_Generic_IEnumerableU3CTU3E_GetEnumerator_m015AB5F84405FCECDAC6FF1A3D303264E46BDEF1_AdjustorThunk','_Enumerator_MoveNext_m4E028544E84BDE88D01F3010D8CA64D7216D5628_AdjustorThunk','_NativeArray_1_GetHashCode_m53AB57C6EDFD1D69493AC0257E005750B7FFDCE5_AdjustorThunk','_NativeArray_1_System_Collections_Generic_IEnumerableU3CTU3E_GetEnumerator_mF62EA42C2AA3B9A8541B491B0367616CC0518FEE_AdjustorThunk','_Enumerator_MoveNext_m76E380AB6772F25135EE9503D3372BA9E13AA7AA_AdjustorThunk','_NativeArray_1_GetHashCode_mD8C51A15BEE95ACFB7BFDEF52FAC04BB36F0B91F_AdjustorThunk','_NativeArray_1_System_Collections_Generic_IEnumerableU3CTU3E_GetEnumerator_m34F3A68AF81DF70A11D22C3CD489E9ED46C23839_AdjustorThunk','_Enumerator_MoveNext_m20DB6EB722DF642E2DE5243BD8728ECE54B1C043_AdjustorThunk','_NativeArray_1_GetHashCode_m3582B57101B5BB52D10BF20AA58B40467524E366_AdjustorThunk','_NativeArray_1_System_Collections_Generic_IEnumerableU3CTU3E_GetEnumerator_mED67002909AA3CC57A54F6B33A441552646BDE7A_AdjustorThunk','_Enumerator_MoveNext_m0B393B0E1E0F5C1408BAD783B0D05353E0E9AB52_AdjustorThunk','_NativeArray_1_GetHashCode_m967A2BBF96740000DD4CBF08E12A7E826C37C5D5_AdjustorThunk','_NativeArray_1_System_Collections_Generic_IEnumerableU3CTU3E_GetEnumerator_m464CABA956FF05E09745425CF40B7888A1A2B441_AdjustorThunk','_Enumerator_MoveNext_m1DCA7A5EC57D1A847891899C5E67645EC1A14BF5_AdjustorThunk','_NativeArray_1_GetHashCode_mAAC3E016D343A908EF5814DAF4BC27F511539783_AdjustorThunk','_NativeArray_1_System_Collections_Generic_IEnumerableU3CTU3E_GetEnumerator_m313E5F3064E843DA8AA2A561F0B6287164447EE9_AdjustorThunk','_Enumerator_MoveNext_m08EAB788EF9356502BB7DC0B527C28401B796E35_AdjustorThunk','_NativeArray_1_GetHashCode_m75EE3771F9EB84A6B37970DE204D5516AEC33C46_AdjustorThunk','_NativeArray_1_System_Collections_Generic_IEnumerableU3CTU3E_GetEnumerator_m60D5495C1A28FD4ED1C09EFD6CAFE6303FA0527F_AdjustorThunk','_Enumerator_MoveNext_mBA68DD436543E0602F8A879BCFB8574E00442459_AdjustorThunk','_NativeArray_1_GetHashCode_mE0FCE180A751227805C844C352B7850B2700C609_AdjustorThunk','_NativeArray_1_System_Collections_Generic_IEnumerableU3CTU3E_GetEnumerator_mAAF9AD471A5F8C99F6BE7C0ECFBAD8A565331188_AdjustorThunk','_Enumerator_MoveNext_m3820998DE6E4C2FC9C2F13823D3AB349A7001926_AdjustorThunk','_NativeArray_1_GetHashCode_m99F2776A02AFF04B5E561AD5A4E83A074017506C_AdjustorThunk','_NativeArray_1_System_Collections_Generic_IEnumerableU3CTU3E_GetEnumerator_m83544A571A28CB2DD639462512DFE0FE7AB82B58_AdjustorThunk','_Enumerator_MoveNext_m27AAB86651AC466F4770FD7402A3F2383D7D5CD1_AdjustorThunk','_NativeArray_1_GetHashCode_mA68BAF11E658B9BD088EE7E9249A11FBCF6A0104_AdjustorThunk','_NativeArray_1_System_Collections_Generic_IEnumerableU3CTU3E_GetEnumerator_m3823F9AE31A9CE5181C2BCD7A5DC7FC2557F672A_AdjustorThunk','_Enumerator_MoveNext_mD716D24CA4C0AEA7731D0009FBCBDD3480E98DC1_AdjustorThunk','_NativeArray_1_GetHashCode_mECAD8FC63FD2153E6F5514C6DC965DB2FD2C07F6_AdjustorThunk','_NativeArray_1_System_Collections_Generic_IEnumerableU3CTU3E_GetEnumerator_m92B9B3FB4E72ABE7C1BD8D0102B765BE4D21494D_AdjustorThunk','_Enumerator_MoveNext_mF6850FF6793A654346743B6F8DEBACDC428F8817_AdjustorThunk','_NativeArray_1_GetHashCode_mF2133A8BF0C0F3DDAA816AAF25E105529107D6F3_AdjustorThunk','_NativeArray_1_System_Collections_Generic_IEnumerableU3CTU3E_GetEnumerator_mB5C68251AFB357F10185D0DB814B065D69CC0B13_AdjustorThunk','_Enumerator_MoveNext_m79A62FCF8983C66AD702851CA3C7ED4A41B26C80_AdjustorThunk','_NativeArray_1_GetHashCode_mD6278FDBDBA6EECB0109F94A0EF7B126A2B6F5C5_AdjustorThunk','_NativeArray_1_System_Collections_Generic_IEnumerableU3CTU3E_GetEnumerator_m262D8ECFA7CE75A6A8E6D2660A63EA7EBF2F0F94_AdjustorThunk','_Enumerator_MoveNext_m696FC72BCD74D6764807F409C49AE24264646E37_AdjustorThunk','_NativeArray_1_GetHashCode_mDAA72F0E5D4B0917DCEDF2234A67BF065CBF5EAD_AdjustorThunk','_NativeArray_1_System_Collections_Generic_IEnumerableU3CTU3E_GetEnumerator_m04093536729D08DF099971303CAFC0D7711500ED_AdjustorThunk','_Enumerator_MoveNext_mBAE60FE5064DB103F75993BEC7AED9484E35E9B3_AdjustorThunk','_NativeArray_1_GetHashCode_mDD91EDED67A5949B4D788FCA68E099788722A5B6_AdjustorThunk','_NativeArray_1_System_Collections_Generic_IEnumerableU3CTU3E_GetEnumerator_m7743CAC6E976ECD02BA6A97277664569ACD2E58D_AdjustorThunk','_Enumerator_MoveNext_mB060B4B05DB23C11885B6AA5AE98FF33C4FFB418_AdjustorThunk','_NativeArray_1_GetHashCode_m24E2443C6EFC50EE8B50584105054A0FCF02F716_AdjustorThunk','_NativeArray_1_System_Collections_Generic_IEnumerableU3CTU3E_GetEnumerator_m4838FFD5C532A24BEA26FDD98B8D0563750A3F9D_AdjustorThunk','_Enumerator_MoveNext_m844C6ABC8F1C0EE62E0382EEF4C22BDE95998176_AdjustorThunk','_NativeArray_1_GetHashCode_m3BAFC3EAABE3CF4517BF606C652705B720ED01E8_AdjustorThunk','_NativeArray_1_System_Collections_Generic_IEnumerableU3CTU3E_GetEnumerator_m413DDB32E27806E4C44C46B27023A9B00A5D6978_AdjustorThunk','_Enumerator_MoveNext_m74D6DEC95648C8659C98CB5C28CAA5489190F236_AdjustorThunk','_NativeArray_1_GetHashCode_m6183D33A22EC9E1B181D3946D4942CD6958D54FE_AdjustorThunk','_NativeArray_1_System_Collections_Generic_IEnumerableU3CTU3E_GetEnumerator_mC93A617F0A764546D2D551508F700E973DD20226_AdjustorThunk','_Enumerator_MoveNext_mA714BE83ABF1ACF9968E68ED752A72EF6807272E_AdjustorThunk','_NativeSlice_1_GetHashCode_mBA5641011EEB465ABBD2F3E1A75038C12F930C10_AdjustorThunk','_NativeSlice_1_System_Collections_Generic_IEnumerableU3CTU3E_GetEnumerator_m96D9ABA73F26962E83ED805C7FBEF46E1D93B397_AdjustorThunk','_BlobAssetReference_1_GetHashCode_mD08A441715EB8CD3BEB4349B409231892AD3E278_AdjustorThunk','_BlobAssetReference_1_GetHashCode_mD006A90F6FEE14ACE07420BD056D267D0585FD2D_AdjustorThunk','_BlobAssetReference_1_GetHashCode_mD8D0F4377556E8D5277AE915687ADD7CA2056AF9_AdjustorThunk','_BlobAssetReference_1_GetHashCode_m5A7F89434EEA30CDF3ED60079827BB6CD549A86F_AdjustorThunk','_U3CU3Ec__DisplayClass_OnUpdate_LambdaJob0_PrepareJobAtScheduleTimeFn_Gen_mD92C2041517B18E84D000DFCC3DB4FB91FAB5559_AdjustorThunk','_U3CU3Ec__DisplayClass_OnUpdate_LambdaJob0_GetExecuteMethod_Gen_mFF31490DA9D162260F97710EF3F10F8D158539E4_AdjustorThunk','_U3CU3Ec__DisplayClass_OnUpdate_LambdaJob0_PrepareJobAtScheduleTimeFn_Gen_m65877E760F0BD677DB200CC4A7212690D44D5F83_AdjustorThunk','_U3CU3Ec__DisplayClass_OnUpdate_LambdaJob0_GetExecuteMethod_Gen_mE695BC71F0FEBA7AD5A6722BF8E451A0C9064DFC_AdjustorThunk','_U3CU3Ec__DisplayClass_OnUpdate_LambdaJob0_PrepareJobAtScheduleTimeFn_Gen_m26A0F9B94B90289D1D1DDEB54C9621B1876131C4_AdjustorThunk','_U3CU3Ec__DisplayClass_OnUpdate_LambdaJob0_GetExecuteMethod_Gen_m79CC11725BE33B77B02BB43FAA7E991F111BAFEA_AdjustorThunk','_U3CU3Ec__DisplayClass_OnUpdate_LambdaJob0_PrepareJobAtScheduleTimeFn_Gen_m56323EACE5ECA1611A59FFD3F372D592D6E902B6_AdjustorThunk','_U3CU3Ec__DisplayClass_OnUpdate_LambdaJob0_GetExecuteMethod_Gen_m61787BE6815B511E3DF0712A18245910410117FC_AdjustorThunk','_U3CU3Ec__DisplayClass_OnUpdate_LambdaJob0_PrepareJobAtScheduleTimeFn_Gen_mCA5CF814A469E4946525C0CA673B495FE3AF5B55_AdjustorThunk','_U3CU3Ec__DisplayClass_OnUpdate_LambdaJob0_GetExecuteMethod_Gen_m9221E6A728B1CFF063C2363A7353DD4A90556B2A_AdjustorThunk','_U3CU3Ec__DisplayClass_OnUpdate_LambdaJob0_PrepareJobAtScheduleTimeFn_Gen_m76BBECE7885D5ABBD82FFDA931C24F589F5CFAA2_AdjustorThunk','_U3CU3Ec__DisplayClass_OnUpdate_LambdaJob0_GetExecuteMethod_Gen_m310F853AB3F3C6FA1137ECFACFE043A79DA8E4E5_AdjustorThunk','_U3CU3Ec__DisplayClass_OnUpdate_LambdaJob0_PrepareJobAtScheduleTimeFn_Gen_mD89420E1A9372D98669DC4F171AEFEC818D08EAF_AdjustorThunk','_U3CU3Ec__DisplayClass_OnUpdate_LambdaJob0_GetExecuteMethod_Gen_mB5FF699786599CF976870AA82B2FD3A368F4A0C1_AdjustorThunk','_U3CU3Ec__DisplayClass_OnUpdate_LambdaJob0_PrepareJobAtScheduleTimeFn_Gen_m22BC8D8B58064AD70EF2F7345B45916AD94CAC51_AdjustorThunk','_U3CU3Ec__DisplayClass_OnUpdate_LambdaJob0_GetExecuteMethod_Gen_m731E246763A05BE5B1CD683A9FDD0D8C5C22F8DF_AdjustorThunk','_U3CU3Ec__DisplayClass_OnUpdate_LambdaJob0_PrepareJobAtScheduleTimeFn_Gen_m726D542AA52C4A2E2E1C9A438451C8E74CEA7198_AdjustorThunk','_U3CU3Ec__DisplayClass_OnUpdate_LambdaJob0_GetExecuteMethod_Gen_m337C45EC41397C9E83312E6B66B345FDAFF642E7_AdjustorThunk','_GatherComponentDataJob_1_PrepareJobAtScheduleTimeFn_Gen_m86C82632A458B0825667A4F960E67CF659501441_AdjustorThunk','_GatherComponentDataJob_1_GetExecuteMethod_Gen_mFB87FBF0B4533607B1532110C845202538A8BEF3_AdjustorThunk','_GatherEntitiesJob_PrepareJobAtScheduleTimeFn_Gen_mC9EA8FF8355507D44577B21FE4310DF50D467A22_AdjustorThunk','_GatherEntitiesJob_GetExecuteMethod_Gen_mDBE189BB32DA6B90B212F0AB1DEA51767572B116_AdjustorThunk','_SubmitSimpleLitMeshJob_PrepareJobAtScheduleTimeFn_Gen_m38AC4AAC131BB9998B56B9D2FB45C5AB00CC1537_AdjustorThunk','_SubmitSimpleLitMeshJob_GetExecuteMethod_Gen_m09A1362E948CB332B6249BD1D642BC3F2D7ADF3A_AdjustorThunk','_BuildEntityGuidHashMapJob_PrepareJobAtScheduleTimeFn_Gen_m20790F910CEB8EA54229CA7D14B6C6DEB46A8D74_AdjustorThunk','_BuildEntityGuidHashMapJob_GetExecuteMethod_Gen_m9EDCC5EA59F11156D6493765124A1AF5F10C0B4C_AdjustorThunk','_ToCompositeRotation_PrepareJobAtScheduleTimeFn_Gen_m1BD14524FA4DEB8F28DA1163F6CD79BB125B3C2D_AdjustorThunk','_ToCompositeRotation_GetExecuteMethod_Gen_mB366744FCF79553C571E4454E29DFACD7ACDF604_AdjustorThunk','_ToCompositeScale_PrepareJobAtScheduleTimeFn_Gen_m2C720D5633917E9B204EA524348C9569B301D5C1_AdjustorThunk','_ToCompositeScale_GetExecuteMethod_Gen_mC0AFB129E75E2C4A0A3C177B79BB4CA34CDB8125_AdjustorThunk','_UpdateHierarchy_PrepareJobAtScheduleTimeFn_Gen_mB87D837465FAE9EC13627DBB79E75B747A4D4DFC_AdjustorThunk','_UpdateHierarchy_GetExecuteMethod_Gen_m9D18B122D4DB4ED1A141ADBE6FABBCE1DB110D20_AdjustorThunk','_ToChildParentScaleInverse_PrepareJobAtScheduleTimeFn_Gen_m051FCF8EF5EF47B25CEA9E169AD2716C451E6918_AdjustorThunk','_ToChildParentScaleInverse_GetExecuteMethod_Gen_mDAABB8E7FC354B3558D9B3684E58802535DD2AD6_AdjustorThunk','_GatherChangedParents_PrepareJobAtScheduleTimeFn_Gen_mAAEA0FD0B7A5CDD1A6FE295465B005746EEE4F9E_AdjustorThunk','_GatherChangedParents_GetExecuteMethod_Gen_mFF235231C878260D10BD22E4D4FA94EB86624972_AdjustorThunk','_PostRotationEulerToPostRotation_PrepareJobAtScheduleTimeFn_Gen_m195B093FBDC87DAEC5C6C49C449DFF0E5BE27305_AdjustorThunk','_PostRotationEulerToPostRotation_GetExecuteMethod_Gen_m175878B312E13FD0087D32F65E50B33CBE063266_AdjustorThunk','_RotationEulerToRotation_PrepareJobAtScheduleTimeFn_Gen_mC5DBB7F4FB7F6DB81E564233D306B23ED7A65739_AdjustorThunk','_RotationEulerToRotation_GetExecuteMethod_Gen_m3F919B728959CD6F973588FC78EEE34122945066_AdjustorThunk','_TRSToLocalToParent_PrepareJobAtScheduleTimeFn_Gen_m80CD1C7BF8682A145FE6DFA32BECEF3AC6AD4C7E_AdjustorThunk','_TRSToLocalToParent_GetExecuteMethod_Gen_m1F0849B962E0417F604D88CDB7EC63774EFDD898_AdjustorThunk','_TRSToLocalToWorld_PrepareJobAtScheduleTimeFn_Gen_m3415BA474538216A581A1E270D95CF75AFDCD9B6_AdjustorThunk','_TRSToLocalToWorld_GetExecuteMethod_Gen_m3CDA4B3428F4779886F83D4F5E5226D3B7C62800_AdjustorThunk','_ToWorldToLocal_PrepareJobAtScheduleTimeFn_Gen_m21C8981E86F60D1BD57E349CD30DA8D26AA220D9_AdjustorThunk','_ToWorldToLocal_GetExecuteMethod_Gen_mAAE3BC1CFC22889406055A387523D296EC7F985E_AdjustorThunk','_DestroyChunks_PrepareJobAtScheduleTimeFn_Gen_m54C66E741847B0F8E2399F257431C32559B83D52_AdjustorThunk','_DestroyChunks_GetExecuteMethod_Gen_m16E91E244726B2DE6A2511BDD0C7D1B7B97C19B9_AdjustorThunk','_SegmentSortMerge_1_PrepareJobAtScheduleTimeFn_Gen_m95761CEE2346D82E0E517713D9EB1962AC314372_AdjustorThunk','_SegmentSortMerge_1_GetExecuteMethod_Gen_m8600198BAD54095ABB572DA918C7DBDA48CF95FA_AdjustorThunk','_CalculateEntityCountJob_PrepareJobAtScheduleTimeFn_Gen_mEBC74570D54BC5CA0C72C0C10729C86736EE2B23_AdjustorThunk','_CalculateEntityCountJob_GetExecuteMethod_Gen_mE61547384E6777162C498DEF088DCCF74BFE889F_AdjustorThunk','_EntityBatchFromEntityChunkDataShared_PrepareJobAtScheduleTimeFn_Gen_m5359E3E47EBB49B1C6723F407C6DD3DD46B42DA9_AdjustorThunk','_EntityBatchFromEntityChunkDataShared_GetExecuteMethod_Gen_mADBC1323F50D918308784DA5D4A853A32EF2170C_AdjustorThunk','_ChunkPatchEntities_PrepareJobAtScheduleTimeFn_Gen_m82BF15AC2A1638552EE0FD1465322E21CC8BF177_AdjustorThunk','_ChunkPatchEntities_GetExecuteMethod_Gen_m916C321D0C07BAE6AD6A4E46E25F54837DD95D21_AdjustorThunk','_MoveAllChunksJob_PrepareJobAtScheduleTimeFn_Gen_m395357651D0B27F39D43669A67EB98D31AFBE62A_AdjustorThunk','_MoveAllChunksJob_GetExecuteMethod_Gen_m35F87D05664A4F006F6668F3D7FEEAF6768F7ECD_AdjustorThunk','_MoveChunksBetweenArchetypeJob_PrepareJobAtScheduleTimeFn_Gen_m54A45FB7F67C9E3AFB4CEC0D9E5A3BDFEA2D96A4_AdjustorThunk','_MoveChunksBetweenArchetypeJob_GetExecuteMethod_Gen_m9988F24DB9B9E8C5A9036D7B1C14AFF6C5BA99AC_AdjustorThunk','_MoveChunksJob_PrepareJobAtScheduleTimeFn_Gen_mC443FFAD4237BF70FE3070FF2E6D0C7783A445E8_AdjustorThunk','_MoveChunksJob_GetExecuteMethod_Gen_mD177DF2E67BE7D26B7DC023EA3FD9D7D4D5D354D_AdjustorThunk','_GatherChunksAndOffsetsJob_PrepareJobAtScheduleTimeFn_Gen_m02EED845D0A650A87FE89641BA29903D0A6D5131_AdjustorThunk','_GatherChunksAndOffsetsJob_GetExecuteMethod_Gen_m67943DDCD581BEB2480AFEDAF69C290A97D81466_AdjustorThunk','_GatherChunksAndOffsetsWithFilteringJob_PrepareJobAtScheduleTimeFn_Gen_m35DF6E7EA0D9B95BD82EC56E397251A07B85D218_AdjustorThunk','_GatherChunksAndOffsetsWithFilteringJob_GetExecuteMethod_Gen_m55E40FE8F8B9BECFFDC270D1DB42038425AB05D0_AdjustorThunk','_FindMissingChild_PrepareJobAtScheduleTimeFn_Gen_m105722506954B808FAC0FE34C1CBD18505E26AA9_AdjustorThunk','_FindMissingChild_GetExecuteMethod_Gen_mF46DCD52EF6642CC4FAA54D8158A9EC935F42063_AdjustorThunk','_FixupChangedChildren_PrepareJobAtScheduleTimeFn_Gen_m5F2F88DF627703368DF77FCF519EC277D4024A26_AdjustorThunk','_FixupChangedChildren_GetExecuteMethod_Gen_mD1BB573ACE350E1D17F65F31E4444E1A4DE099CB_AdjustorThunk','_GatherChildEntities_PrepareJobAtScheduleTimeFn_Gen_m75E4EF5AFEA08A6C103D0187ADA7687D17F3272D_AdjustorThunk','_GatherChildEntities_GetExecuteMethod_Gen_m4E82C7D9736017F1CB0CF92CC56D1D80F59C0465_AdjustorThunk','_SegmentSort_1_PrepareJobAtScheduleTimeFn_Gen_mA00EFF17DA1AED5C3CCF7E4E5AFD9EFFF9B367C4_AdjustorThunk','_SegmentSort_1_GetExecuteMethod_Gen_m51E07745331F934F53266A7D86C3983ECBC27FD2_AdjustorThunk','_GatherEntityInChunkForEntities_PrepareJobAtScheduleTimeFn_Gen_m8753653DFF57A103D0703E55000FD5718349130C_AdjustorThunk','_GatherEntityInChunkForEntities_GetExecuteMethod_Gen_mC786F10E65430307BB03B12FEFB44EE587A4A1DD_AdjustorThunk','_RemapAllChunksJob_PrepareJobAtScheduleTimeFn_Gen_m8BECB15B4EA058B6347980F80DE00C78B6E40626_AdjustorThunk','_RemapAllChunksJob_GetExecuteMethod_Gen_m1881CA08D884F88FA9A63A9C6E842D0844F3CDB6_AdjustorThunk','_RemapArchetypesJob_PrepareJobAtScheduleTimeFn_Gen_mA8821B4E9A1692A2B96B4BB45EB11178FA1BE451_AdjustorThunk','_RemapArchetypesJob_GetExecuteMethod_Gen_mCD7A9D010A60696B2A7F611C25F8DB49140CBA32_AdjustorThunk','_RemapChunksJob_PrepareJobAtScheduleTimeFn_Gen_mD171B6031F84FF071F49AA2FE0C87417A922803E_AdjustorThunk','_RemapChunksJob_GetExecuteMethod_Gen_m3C010BFAE9B1B4A5989DE2DC457A72F9CABD2B9C_AdjustorThunk','_RemapManagedArraysJob_PrepareJobAtScheduleTimeFn_Gen_m0B5C2144B9692C9FF5E4B5D3B04D863D78554562_AdjustorThunk','_RemapManagedArraysJob_GetExecuteMethod_Gen_m73FB822A7595278347E17FB3E9FA852152DBD50A_AdjustorThunk','_GatherChunks_PrepareJobAtScheduleTimeFn_Gen_m17E2A5CD847201794983710C48151D1674425951_AdjustorThunk','_GatherChunks_GetExecuteMethod_Gen_mCF09FAF4A2EBF6C1ABDFA83CAC17A46C907864D6_AdjustorThunk','_GatherChunksWithFiltering_PrepareJobAtScheduleTimeFn_Gen_mBC7477B0B6864139B2594B2B86F1CA218D6F6856_AdjustorThunk','_GatherChunksWithFiltering_GetExecuteMethod_Gen_m055D975760379D0563862F1F35246848534F3509_AdjustorThunk','_JoinChunksJob_PrepareJobAtScheduleTimeFn_Gen_mA890678AA535B005A0AEFE5DCAE3C8CAA58A3C7D_AdjustorThunk','_JoinChunksJob_GetExecuteMethod_Gen_m35269015F2DF91F3C693C26086C001FD6F7038B1_AdjustorThunk','__ZNK4bgfx2gl17RendererContextGL15getRendererTypeEv','__ZNK4bgfx2gl17RendererContextGL15getRendererNameEv','__ZN4bgfx2gl17RendererContextGL15isDeviceRemovedEv','__ZN2bx17StaticMemoryBlock7getSizeEv','__ZN4bgfx4noop14rendererCreateERKNS_4InitE','__ZN4bgfx4d3d914rendererCreateERKNS_4InitE','__ZN4bgfx5d3d1114rendererCreateERKNS_4InitE','__ZN4bgfx5d3d1214rendererCreateERKNS_4InitE','__ZN4bgfx3gnm14rendererCreateERKNS_4InitE','__ZN4bgfx3nvn14rendererCreateERKNS_4InitE','__ZN4bgfx2gl14rendererCreateERKNS_4InitE','__ZN4bgfx2vk14rendererCreateERKNS_4InitE','__ZNK4bgfx4noop19RendererContextNOOP15getRendererTypeEv','__ZNK4bgfx4noop19RendererContextNOOP15getRendererNameEv','__ZN4bgfx4noop19RendererContextNOOP15isDeviceRemovedEv','___stdio_close','_U3CU3Ec__DisplayClass0_0_U3CMainU3Eb__0_m38308E5629152C6F37DDB1F8B7C2F30141860823','__ZL10RevealLinkPv','__ZN6il2cpp2gc19AppendOnlyGCHashMapIKlP20Il2CppReflectionTypeNS_5utils15PassThroughHashIlEENSt3__28equal_toIS2_EEE10CopyValuesEPv','_emscripten_glCheckFramebufferStatus','_emscripten_glCreateShader','_emscripten_glGetString','_emscripten_glIsBuffer','_emscripten_glIsEnabled','_emscripten_glIsFramebuffer','_emscripten_glIsProgram','_emscripten_glIsRenderbuffer','_emscripten_glIsShader','_emscripten_glIsTexture','_emscripten_glIsQueryEXT','_emscripten_glIsVertexArrayOES','_emscripten_glIsQuery','_emscripten_glUnmapBuffer','_emscripten_glIsVertexArray','_emscripten_glIsSync','_emscripten_glIsSampler','_emscripten_glIsTransformFeedback',0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0];
var debug_table_iid = [0,'_Double_CompareTo_m2204D1B6D890E9FE7299201A9B40BA3A59B80B75_AdjustorThunk','_Double_Equals_mA93F2BE22704B8C9EB96046B086ECA4435D642CA_AdjustorThunk',0];
var debug_table_iif = [0,'_Single_CompareTo_mD69065F0577564B853D364799E1CB0BA89D1B3A2_AdjustorThunk','_Single_Equals_m695797809B227FBC67516D4E43F661CE26325A86_AdjustorThunk',0];
var debug_table_iii = [0,'_ValueType_Equals_mEE494DD557D8885FC184A9ACB7948009A2B8A2FF','_Object_Equals_mA588431DA6FD1C02DAAC5E5623EF25E54D6AC2CF','_String_Equals_m8EF21AF1F665E278F58B8EE2E636501509E37420','_Int32_Equals_mF0C734DA2537887C0FB8481E97B441C6EFF94535_AdjustorThunk','_Int32_CompareTo_mCC31C7385E40B142951B542A7D002792A32E8656_AdjustorThunk','_NumberFormatInfo_GetFormat_mD0EB9E76621B46DE10D547A3CE10B64DE2D57A7F','_UInt32_Equals_m9FC90177169F42A34EFDDC393609A504CE67538A_AdjustorThunk','_UInt32_CompareTo_m2F3E12AD416BA8DCE08F5C54E9CABAFB94A18170_AdjustorThunk','_Guid_Equals_m5CFDE98D8F0D0666F0D63DEBB51CDF24AD891F40_AdjustorThunk','_Guid_CompareTo_m635746EA8CED3D4476CE74F8787310AFC57AEFC0_AdjustorThunk','_Guid_Equals_m4E37FD75580BEC68125508336F314F7D42997E1D_AdjustorThunk','_IntPtr_Equals_m4F97A76533CACEECD082EF639B3CE587CF9146B0_AdjustorThunk','_Enum_Equals_m18E82B9196EBA27815FA4BBE1A2A31E0AFCB8B54','_SByte_Equals_m5C1251272315CA14404DB1417B351B8489B89B96_AdjustorThunk','_SByte_CompareTo_mA406A19828A323C071A676F8ABDF1522982A71F8_AdjustorThunk','_Byte_Equals_m9149D4BDB8834AD79F18A3B973DEF5C050B855D2_AdjustorThunk','_Byte_CompareTo_m901D408ED147198D917F7AB0A0C4FA04B1A8AA32_AdjustorThunk','_Int16_Equals_mD04B4E653666D8266CFD21E1ADD9D466639BA890_AdjustorThunk','_Int16_CompareTo_m664B140D73E6B09CE806A689AA940D14C150B35F_AdjustorThunk','_UInt16_Equals_m73308B26E6618109710F039C7BB8E22CE5670529_AdjustorThunk','_UInt16_CompareTo_mC7B898354424F5CA6066F3AF0A3276D1A71C27F5_AdjustorThunk','_UIntPtr_Equals_m28C138F952F22CFBC3737208ADA93F05B8804802_AdjustorThunk','_bool3_Equals_mF8096E80ED67BF96FF5AFF7781E0DAE080976ABA_AdjustorThunk','_bool3_Equals_mBEDD70C4301F56A2FB7DB9ECB24BD3113959979F_AdjustorThunk','_bool4_Equals_m16C6A83ED61ACF4A3B18296B5CD8AC87354B2185_AdjustorThunk','_bool4_Equals_m8CA8401F2096436C18CDD4DC003BED60265AFC5E_AdjustorThunk','_float2_Equals_mB9C9DA2AF09FF68054FE96FC54BF5256D8812FD9_AdjustorThunk','_float2_Equals_m7B70628801F4833DAB85E08DE01B853E1BAB3B01_AdjustorThunk','_float4_Equals_m9D39B0C2F3B258DFE32BC4DF9C336CA53FB01C8C_AdjustorThunk','_float4_Equals_m304B8FCAD7E6F0A7F0B5627F264F4A85E824FA21_AdjustorThunk','_float3_Equals_mE47DABC0C9A780512ED16E16AEF8BC281DD4830C_AdjustorThunk','_float3_Equals_mD907D4D448B5C8F48E8A80990F482F77A57DF520_AdjustorThunk','_float3x3_Equals_mFE36EBED6FDB5DA7AE80F8508EB51DF5F48C86CE_AdjustorThunk','_float3x3_Equals_m7F751F6F5B0009FB462E989800A234ECBC9D8DF3_AdjustorThunk','_uint3_Equals_m42E00C7EAD53725C48642FA60CEBAC62C33C24E9_AdjustorThunk','_uint3_Equals_mA68ACCC408ACA27FBF6A04D330906B2D6611D919_AdjustorThunk','_float4x4_Equals_mEC3A38C4484251F997A1AE94FCBB12626077D3E6_AdjustorThunk','_float4x4_Equals_mBAF370F397DEC9CEA58FF78FBF68E3813FD3A88E_AdjustorThunk','_uint4_Equals_m0A07A846236F3F0D5C37D221617D693CAD333AEF_AdjustorThunk','_uint4_Equals_m0A69791A8BCBEE1532F40BC5C28C48A1496A2588_AdjustorThunk','_uint2_Equals_m486320DA825FC95194D5831B96E52DB113CC023F_AdjustorThunk','_uint2_Equals_m92043463D1AF6F25D28BD6C1FBD20686899886FD_AdjustorThunk','_il2cpp_virtual_remap_enum1_equals','_quaternion_Equals_mB9B9BF3C94A7D7D555825FB54B64B02DCB89A151_AdjustorThunk','_quaternion_Equals_mC9DC919B846AEE486EE21CB92E451F45841A3447_AdjustorThunk','_il2cpp_virtual_remap_enum4_equals','_NativeString512_Equals_mC5C459E3D016F3700ED0A996F89AA0288C6D4074_AdjustorThunk','_NativeString512_CompareTo_m359B652FB19E397A83121085E8DBD493AADF2606_AdjustorThunk','_NativeString512_Equals_mCF1E64EED1A677B16B3C60481051EE7897AF1EDD_AdjustorThunk','_EntityQueryBuilder_Equals_mBC180CB5BB4B5687A65496C86ACF116BEE5E4325_AdjustorThunk','_ComponentType_Equals_m97C28B3743F1C228712C0E775D952BA181A997E4_AdjustorThunk','_ComponentType_Equals_mB92EC274A59380214CA9BE66B61532AAFF2F5F72_AdjustorThunk','_NativeArray_1_Equals_m6F5978892D485FD36AEC1F90CFD5AB5466934B17_AdjustorThunk','_NativeArray_1_Equals_m0580C4DE5F6FC28F25E729014FE7F0961AA904F4_AdjustorThunk','_NativeArray_1_Equals_m2C603577039C36A0F6AEDDCA4BF59FC7515CEA91_AdjustorThunk','_NativeArray_1_Equals_mA482F46879E2F6A6E93BBDDB3BEE4D0D4CA2F430_AdjustorThunk','_Entity_Equals_m8B9159BC454CEA2A35E9674B60B3CEF624F5C6F3_AdjustorThunk','_Entity_Equals_m2739CD319AB17A7318B7DF9D29429494E6036D01_AdjustorThunk','_Entity_CompareTo_mBA83E2FCC310A03CA53B7E2580C1CE5F9101B58C_AdjustorThunk','_Scene_Equals_mE2C85635DAE547EA1B63AEA7805B006D7D0C4E93_AdjustorThunk','_Scene_Equals_mF5A38E847AD1BD6AF0A3F4D140A4486E10A34A19_AdjustorThunk','_SceneGuid_Equals_mDEF0B9DA1FAABDC9EDBA6AE4FE9793A5B9DA2CFA_AdjustorThunk','_SceneGuid_Equals_mB22F600C66019AC5805763DD7A0B5D8F6D78C381_AdjustorThunk','_EntityGuid_Equals_mDFE00740AF93F8287164B0E268E1816E00FBFDED_AdjustorThunk','_EntityGuid_Equals_m1BF7F17598B3CDE5454CB7295B5AD78BD047CCC4_AdjustorThunk','_EntityGuid_CompareTo_mEDEFCFBCAF4D468B3FA58B11C3C92A51BF68BC7C_AdjustorThunk','_SceneReference_Equals_mBB4A710D9D4B79A5853484BAF0941AA10C5635F6_AdjustorThunk','_SceneTag_Equals_m3EFAF1C15796A3A5E0EB6D30A42DAE783F8C8A24_AdjustorThunk','_SceneSection_Equals_m94C65474CC395168100176CE8E31F4CBAD124CC6_AdjustorThunk','_SimpleMaterial_Equals_m4BFED00024CB1D0E65DCEEA2B358329C729D7637_AdjustorThunk','_LitMaterial_Equals_mF674981FA2EDCC1514DA77F89A74ADAC21FF6AED_AdjustorThunk','_BuildGroup_Equals_mE8181934EB50B4F5F184E9F27D29BFB83FC1A41B_AdjustorThunk','_SortSpritesEntry_CompareTo_m4CEDDAA899EDFE33AA4E17AE29575D014CC3FF0D_AdjustorThunk','_AudioHTMLSystem_PlaySource_mFC7C3FCB2C78ADE1432AE215EB11E0F9CBC98013','_AudioHTMLSystem_IsPlaying_mA8FD56A9A2AF8BA3807B4AD6A3A5CB0C169660EB','_EntityArchetype_Equals_m6DD973EED29BF29894D6C4758F06F304F9B40322_AdjustorThunk','_EntityArchetype_Equals_mF4919F60F03979435FC6A009C807231C4F39D815_AdjustorThunk','_EntityInChunk_CompareTo_m77C233D22BA7265BA0CB2FAFE346264E4890F37D_AdjustorThunk','_EntityInChunk_Equals_m2C322B7C39EA488BADDBD6A35AF7F146F243879C_AdjustorThunk','_ComponentTypeInArchetype_Equals_m55D46DCBEAC64BF2703ED99BFC6DFF51BBACF97F_AdjustorThunk','_ArchetypeChunk_Equals_mB60BAA8621FA93E12D76B156DB1F5F059009AD5F_AdjustorThunk','_ArchetypeChunk_Equals_mC90EE0E63C788B66064CEA02BF1BE20348462EEC_AdjustorThunk','_BlobAssetPtr_Equals_m1D07B3C19EB26C534A5058AD6A8335E0F3C48391_AdjustorThunk','_BlobAssetPtr_Equals_m02270937419C556F4CD01A6769297BB24F847D16_AdjustorThunk','_BlobAssetPtr_CompareTo_m07718073C78567CEAF2E5F8D6DF07E98481D17F1_AdjustorThunk','_GetSystemType_1_Invoke_m534FF49A3221F32616927F97361FD9185F9914B8','_NativeArray_1_Equals_m20C38F6A75248F77D80270E1C050210A347F8062_AdjustorThunk','_NativeArray_1_Equals_m7F122EC5FED8436D16EF288C0D7F29372504FCED_AdjustorThunk','_NativeArray_1_Equals_mFE3C41BFB546290B87BC249494197B04C1E489F5_AdjustorThunk','_NativeArray_1_Equals_mF5AC0CAF03FDAA5CDB10DDF1C6A1EB5BDAF8BFBB_AdjustorThunk','_Hash128_Equals_m10DF98E630E98B91BBFAAD9DDF4EDB237273A206_AdjustorThunk','_Hash128_Equals_mC53374D67521CD5C4413087C1F04880D870B2C34_AdjustorThunk','_Hash128_CompareTo_m56E2D65F12FEAE043EA9753C8F1D99DB480EE0FA_AdjustorThunk','_NativeArray_1_Equals_m61C847C1DF82DFAE7E19C9A1052C7F5F63A8C204_AdjustorThunk','_NativeArray_1_Equals_m7099C1223442CA106E550FFA129F90E03F745111_AdjustorThunk','_Color_Equals_m9BAA6F80846C3D42FD91489046628263FD35695E_AdjustorThunk','_Color_Equals_m4BE49A2C087D33BAACB03ECD8C9833AB1E660336_AdjustorThunk','_NativeArray_1_Equals_m29FD5DF54C0B9C122C02090F2ED6A51B0D196C53_AdjustorThunk','_NativeArray_1_Equals_m592E02E164E79DD90AF5DC1E7BA9A8EA9DE1166B_AdjustorThunk','_NativeArray_1_Equals_m302B6BEF84C12946BC013C0EB702A0607BD59727_AdjustorThunk','_NativeArray_1_Equals_mFAF9006CEE962F0E7B7BC1CC4E07F393C3CBA546_AdjustorThunk','_NativeArray_1_Equals_m5429614F2C316D010ED567A94A866CFCABEB1CDF_AdjustorThunk','_NativeArray_1_Equals_mC62713DC672B3B56B6953C80245A90F247CF148C_AdjustorThunk','_NativeArray_1_Equals_m70013632FB1602568F08D581673EBB507E58C449_AdjustorThunk','_NativeArray_1_Equals_mD49A254E7FDFF8838A19752DDA6FA989F37D77BA_AdjustorThunk','_NativeArray_1_Equals_m8F22E0D94A50B4C0E0CF99E4BF1F259A679D582F_AdjustorThunk','_NativeArray_1_Equals_m25F8AB7E862EC503EC2F5C8514F935258A113B87_AdjustorThunk','_NativeArray_1_Equals_m549868F3D3ED7F86C3BDB56C9E8DDE32BD6BCC7B_AdjustorThunk','_NativeArray_1_Equals_m3400B270668BACD40C1318F91FDECEFB4AB51327_AdjustorThunk','_NativeArray_1_Equals_mAA1D637458E10D0CD6CFF5CE8CFC1C953E4C2844_AdjustorThunk','_NativeArray_1_Equals_m5794501BB5D0AA066ADDC59D3420B4F23BE28C03_AdjustorThunk','_NativeArray_1_Equals_m80551616037DD63FCD1FFCF6B41331CB84F2FAB4_AdjustorThunk','_NativeArray_1_Equals_mA7A8F3F2CBAC4B9E812F92434DBB550CD3D4E34E_AdjustorThunk','_NativeArray_1_Equals_m9E1696F40D6CEE43B465A16510343657780F2BA3_AdjustorThunk','_NativeArray_1_Equals_mE3A1FC7FC129CD78E5D9622A92B9C6427AAD722D_AdjustorThunk','_NativeArray_1_Equals_m971476226D9BCEC8E80E9149FA11DE6B92442058_AdjustorThunk','_NativeArray_1_Equals_mACF819D9B93A912FF9C32EEA1E4D5F1AD69CCC5B_AdjustorThunk','_NativeArray_1_Equals_m5A96F77458BB195F7CD6E9CCF73861178746CDFC_AdjustorThunk','_NativeArray_1_Equals_m5E90AA69C438D07593E75EFD5ED6FBB359BB4541_AdjustorThunk','_NativeArray_1_Equals_mEAC40ED11EE83896D7E55AFC3F9A71506523E57B_AdjustorThunk','_NativeArray_1_Equals_mA5EEA2C734AF38E8473805F1AB872B48A866AFF7_AdjustorThunk','_NativeArray_1_Equals_m4C33AB9E66C8A316744DD50539241AFC427A848C_AdjustorThunk','_NativeArray_1_Equals_m48BA0EE86C00604C07241522DD22D3A0283AB124_AdjustorThunk','_NativeArray_1_Equals_m01CD6A6067019D475A36BF993C024AD59AE316AF_AdjustorThunk','_NativeArray_1_Equals_m20AD397D6C86F68A37BA82A11B60FC34957FEC03_AdjustorThunk','_NativeArray_1_Equals_m7E602BEDC6D695BB65A7FFFC9C8F007EC0E7597B_AdjustorThunk','_NativeArray_1_Equals_m281AF6743B6072BE9F9B6CEE73C522D2088FC5F2_AdjustorThunk','_NativeArray_1_Equals_m72B88F597F794E4D65C2DADCCC3033BA18B5DE7A_AdjustorThunk','_NativeArray_1_Equals_m74DF4FDD3C58A391C4DEA411ACC562172F984765_AdjustorThunk','_NativeArray_1_Equals_mCDD378D700D08029AADA61E3F229CE99265770A1_AdjustorThunk','_NativeArray_1_Equals_m1363B76E515D5F986DC51FC43E0CD3C4E2C25B78_AdjustorThunk','_NativeArray_1_Equals_m9E4DC18C694A1521C33804261012E4B7B14E6E23_AdjustorThunk','_NativeArray_1_Equals_m40166A802E883EBF355B615F727083AD3BD040EF_AdjustorThunk','_NativeArray_1_Equals_m46A64D4607FA37FF8EFC03995F8DF015F3E02F53_AdjustorThunk','_NativeArray_1_Equals_m2A0031FBFA9C27B9F73A647BD905DF65C6253192_AdjustorThunk','_NativeArray_1_Equals_m0EDA2DDFCC16C418A749226A8E201EDC51FEDE78_AdjustorThunk','_NativeArray_1_Equals_m61C93E4321016E0AF8FCA6F70203FEDB0ADACEA0_AdjustorThunk','_NativeArray_1_Equals_m109FBF86AAB3AD66F7EF45A80B126CB7ACBC9C4D_AdjustorThunk','_NativeArray_1_Equals_mC382E0F0FDB47680CC07CA9178493C25C90CC64B_AdjustorThunk','_NativeArray_1_Equals_m8F9BEB1BE5E806C3E1D054864B6657CD83F0AF52_AdjustorThunk','_NativeArray_1_Equals_m1A6A4D4C3BF34B209C7F1D1150EB5A801D731575_AdjustorThunk','_NativeArray_1_Equals_m65664CCC3C664FF015203FFC77CA1F1DDB8E75B7_AdjustorThunk','_NativeArray_1_Equals_m2F3A2BC5B9DE7CF8B94EDFB738ECF5F885ACBC43_AdjustorThunk','_NativeArray_1_Equals_m465B5C9980FD5988C52C0CAEDB4C170B2D604063_AdjustorThunk','_NativeArray_1_Equals_m58A19B454802DE82200E9E746A0A15556E7277D1_AdjustorThunk','_NativeArray_1_Equals_mDEA91902CF0BF2757ED4B005457C79ECC412006B_AdjustorThunk','_NativeArray_1_Equals_m37C01559638609832DB500307CC18FA3B4D746AF_AdjustorThunk','_NativeArray_1_Equals_m8C510A6CE412E552E1375D39B17B5D37E4C0CCE7_AdjustorThunk','_NativeArray_1_Equals_mA9E90059006885EED3821F6783DCCF37F5DEA9BD_AdjustorThunk','_NativeArray_1_Equals_mEECACE9F1D5AE9F618FC5B015D2CB79B57AEFB34_AdjustorThunk','_NativeArray_1_Equals_m63AF341D3940FEB22D6883CA500635C371429FBF_AdjustorThunk','_NativeArray_1_Equals_mABE64DCD2C1B48926067ED675A3CD5BAF5B0D9D4_AdjustorThunk','_NativeArray_1_Equals_m109393A3D8CC8D89A7D72631CBAB4F9C59CBC4F2_AdjustorThunk','_NativeArray_1_Equals_mEE0586D6AFAE2543EA9656C60E07AA9B551BFA2D_AdjustorThunk','_NativeArray_1_Equals_m6D3C8E1A21AB313FD7DC1C36F35F8BD734B0A63A_AdjustorThunk','_NativeArray_1_Equals_mD98309B56895C56125EA6C4859BB4AABF2944D66_AdjustorThunk','_NativeArray_1_Equals_mEA359DF0455F17FD3B4BE09DA7360631E9B172F7_AdjustorThunk','_NativeArray_1_Equals_m634EC99EA48FB36A253CAC9045E3FE83669BB987_AdjustorThunk','_NativeArray_1_Equals_mB5FB2A1CBC844F136203B90420AB4973C0B675C6_AdjustorThunk','_NativeArray_1_Equals_m7F1A0E855A345207A2AB5BFC959047B578F89B9E_AdjustorThunk','_NativeArray_1_Equals_mD6AD32BC9640C21C0EB128B26191DC9F4C26A1F3_AdjustorThunk','_NativeArray_1_Equals_m3326BC381D0E8787AABF2BA935C6F3C04FF5CC2C_AdjustorThunk','_NativeArray_1_Equals_mEA972E3FA3D6BEB78F3B20572723085E0554382F_AdjustorThunk','_NativeArray_1_Equals_m9154F89B3CD0534DC40EC51C2B0B0981972A87D8_AdjustorThunk','_NativeArray_1_Equals_mFE845D78B10E4830D153F9F08DE49D16DEEDA99B_AdjustorThunk','_NativeArray_1_Equals_m05E088BB65A9985D7944269E745C44F3041266AE_AdjustorThunk','_NativeArray_1_Equals_m4E27BD01CF5E85DF4F4F5C5E42FC2F852944C836_AdjustorThunk','_NativeArray_1_Equals_mCFBEDCDEF8EB4D07CCC91A59E6FD39FEDA208816_AdjustorThunk','_NativeArray_1_Equals_m711067BBF1ABB644483C054F0B2A41BAD3DA0E6F_AdjustorThunk','_NativeArray_1_Equals_mBDD98800EB0FAC6E6D821FD96C1ACEDE4F9A3A29_AdjustorThunk','_NativeArray_1_Equals_m524B5C47F086224A205911FB4AACD4E2DF614C22_AdjustorThunk','_NativeArray_1_Equals_m1C914426A82AA3DAD6C5E4618F35572DC2C93264_AdjustorThunk','_NativeArray_1_Equals_m2D91CE4E179AB5088E0E2CC68E0FFD2CEA75F3D1_AdjustorThunk','_NativeArray_1_Equals_mE8E8C56D9697A19AB74B5A56DF82AC7631544392_AdjustorThunk','_NativeArray_1_Equals_mE51D491AAB0B413B62E0F47073352BF33ED5FE69_AdjustorThunk','_NativeArray_1_Equals_m6F060D8A3C6D6C80A8E15B3D448D7C0F92676CE0_AdjustorThunk','_NativeArray_1_Equals_m84EB8E5196E03423B502EB9D1D6FE563C3D3829E_AdjustorThunk','_NativeArray_1_Equals_mC98070EE624560C0BD1D1F982F26750D95944C43_AdjustorThunk','_NativeArray_1_Equals_mA9A145892AD8997700682DBF0D5C5E6560C1ED05_AdjustorThunk','_NativeArray_1_Equals_mFB5BD117BB8ACA6130AAE07DF7530E0E307CF133_AdjustorThunk','_NativeArray_1_Equals_m87F218CDA87F14FB2673B3206FAB44201130D611_AdjustorThunk','_NativeArray_1_Equals_m6E21779CEB29C5B8A204F39FCE447A5551A74591_AdjustorThunk','_NativeArray_1_Equals_mE23801268EEA794473808556B0272B131A61A109_AdjustorThunk','_NativeArray_1_Equals_mD4D2878F875FD067287C72D60655F75A574AAA62_AdjustorThunk','_NativeArray_1_Equals_m1F12DD2B350C2D2DF7A3A1FC8289352BA2B0EF7F_AdjustorThunk','_NativeArray_1_Equals_mC3FF5CE9A3F7E0C0517D20795529F7E51384E6B6_AdjustorThunk','_NativeArray_1_Equals_m83F5B0161C3A2A6D6861AB237D9B4AD232B9F7FA_AdjustorThunk','_NativeArray_1_Equals_m25A863D16C80CCD7A13D64AA5EC32478C7B022F6_AdjustorThunk','_NativeArray_1_Equals_mEEEFE11A79FA0B4FE1F9286E027B99C81126F1C7_AdjustorThunk','_NativeArray_1_Equals_m0E7F184F1B35507D6890C3CE15E653EFBDDB8C30_AdjustorThunk','_NativeArray_1_Equals_m2A4170EE2AAAAA46E2583499CF6F90EAB5DE1543_AdjustorThunk','_NativeArray_1_Equals_mDC0C94074BC53D9AB5CD667536C3F760E371FC1C_AdjustorThunk','_NativeArray_1_Equals_m751551E971E927CDB8936471D66E9C99E3FECC7A_AdjustorThunk','_NativeArray_1_Equals_m89E2B5D9750DD50DBD9BCA13E5D0B0CBBDC88DA2_AdjustorThunk','_NativeArray_1_Equals_m3C0E9DE348D51045B0FDE1D3B7AC72CA7B466F18_AdjustorThunk','_NativeArray_1_Equals_m84AAD28358C891EC5A776355155F1E4B607EF253_AdjustorThunk','_NativeArray_1_Equals_mFE606CFBC6612E59D8072E13584EAD650463BC07_AdjustorThunk','_NativeArray_1_Equals_m0EA7B4D86B22A0547E2B1437F4932BD9F7235036_AdjustorThunk','_NativeArray_1_Equals_m7DEB57FF4CC14B447F22AC85C9929D07BAA7E771_AdjustorThunk','_NativeArray_1_Equals_m684E91BEA6753963A4DC31EE972BC0F181F59D8F_AdjustorThunk','_NativeArray_1_Equals_m7F2A7101E6B6210C60073344D0BE39CEC1FEF2CC_AdjustorThunk','_NativeArray_1_Equals_m430DBA74CE28A604EEEEFFB7536B83ADE0E4420B_AdjustorThunk','_NativeArray_1_Equals_mFE291C8603810777348117A64443DD87EEB4F686_AdjustorThunk','_NativeArray_1_Equals_mC1F22D61B4A9844884C39CB50C193B1CCE130E4B_AdjustorThunk','_NativeArray_1_Equals_mA807A11F91EBC531EB10729096FBEE3BEE9C2592_AdjustorThunk','_NativeArray_1_Equals_mFCE3E8C1E5D1765221E5A0CECBCACDBFA8FE8EBF_AdjustorThunk','_NativeArray_1_Equals_m803C3E83132F106ABFB110D55268B0E7D1F63ABD_AdjustorThunk','_NativeArray_1_Equals_mBCCF21C14746D729F192E1CF85364D1A772A7AD6_AdjustorThunk','_NativeArray_1_Equals_m29A629E6122C4A875E2519FD50E22B68055C2B4A_AdjustorThunk','_NativeArray_1_Equals_mFCF113D15309804F7FAF1C3D9216AF46895C03BB_AdjustorThunk','_NativeArray_1_Equals_m56913B3E4E842627A6765211A791A8B85A1A9C16_AdjustorThunk','_NativeArray_1_Equals_m68DBADA2F56FC6C93C36A522177919965E2BC1D4_AdjustorThunk','_NativeArray_1_Equals_mE52F49F205645A2378069E6BC9AD4BC5F2C8DB49_AdjustorThunk','_NativeArray_1_Equals_m3D5DFA9CBF13D6999C0F76C65D6CFFBC56E5D043_AdjustorThunk','_NativeArray_1_Equals_m458273A988DCAE7B3FC0443BC4A04280887AC9FE_AdjustorThunk','_NativeArray_1_Equals_m4F4E4F67B0141A25287D6B1FBF083F8E29B138E4_AdjustorThunk','_NativeArray_1_Equals_m6459DEC4B8B0E6DACF25AA7F86F43A46914C740B_AdjustorThunk','_NativeArray_1_Equals_mE0273AA92D66A9DF58A570E17693E3D2BE34B909_AdjustorThunk','_NativeArray_1_Equals_mE5F7C246552831EB8D30AC9EC21DDD0C8812CEA5_AdjustorThunk','_NativeArray_1_Equals_m847DEDD8C2289218E6099DB3EB565A49BC493CAE_AdjustorThunk','_NativeArray_1_Equals_m97085C4F8591EDCB0BACF4A6840B2FEC7A5EFE3A_AdjustorThunk','_NativeArray_1_Equals_m22B62B2E97176C6838F9B25D9B83098FCF4DC396_AdjustorThunk','_NativeArray_1_Equals_m51097F46B1CC1C346ED2CCB037B8D9E61E9AA8C1_AdjustorThunk','_NativeArray_1_Equals_m2FB719155EB3934F505ADCDB7E04D3AE57EF7C10_AdjustorThunk','_NativeArray_1_Equals_m6A507FF423375731991BBFAE5B9AF11EB0809755_AdjustorThunk','_NativeArray_1_Equals_m42284045ABE3CAC6CD920DC1CC383D1DB3405F73_AdjustorThunk','_NativeArray_1_Equals_mAFAA05BA50D3E406F8010171AD903079874AEDED_AdjustorThunk','_NativeArray_1_Equals_m7B2963691162B9FEE2F0D43F0566E48B4EE4F83D_AdjustorThunk','_NativeArray_1_Equals_m1EB11E044EA45D09947721EB8F07E408247DDFD4_AdjustorThunk','_NativeArray_1_Equals_m56139F4357F0F1D79D90284D0CABC00D541FD30A_AdjustorThunk','_NativeArray_1_Equals_m4C97FD6C5799DF0CBC2B7BD033E1BCF2F73208D1_AdjustorThunk','_NativeArray_1_Equals_m80A1F1BFD6E35D70CC67779E5C72994E3444B6E4_AdjustorThunk','_NativeArray_1_Equals_m20991547F1B7B83724EE8557B134A680776FDB6F_AdjustorThunk','_NativeArray_1_Equals_m41DBD84EA2954500475D252835B06E5F1B452B28_AdjustorThunk','_NativeArray_1_Equals_mCC1A3D33DACE1503E8D9EA7B81B1659DF5E338C2_AdjustorThunk','_NativeArray_1_Equals_m022FB0F3788C6DE733C512287F026ADD22DB3DE5_AdjustorThunk','_NativeArray_1_Equals_m7556498CDB7551C2ADCD9BC03D572287FA971A88_AdjustorThunk','_NativeArray_1_Equals_m05E3D5E1D5C14635E8BC6A0A0033DB80242521A8_AdjustorThunk','_NativeArray_1_Equals_m254513CD1DCC5A91BBC5A842FEFA35B570102E6C_AdjustorThunk','_NativeArray_1_Equals_m76FDCCC93AA4D257AD9B46F0B0928B6C601AB848_AdjustorThunk','_NativeArray_1_Equals_m87F6134BD13BC5773CFDC05EA0D0568D5B5ED6FF_AdjustorThunk','_NativeArray_1_Equals_mE06E8943B63619BDD07D77B121592ED443F7506D_AdjustorThunk','_NativeArray_1_Equals_m3325AC1E27A6982A10B9BC824A999E983A892B8E_AdjustorThunk','_NativeArray_1_Equals_m2204567A5BB0F5E6829520D66ECD199B1D3E7E19_AdjustorThunk','_NativeArray_1_Equals_mE03AC873516B43755E1764F32AFC3FF157C1F2EB_AdjustorThunk','_NativeArray_1_Equals_m26A335E88D619954A3F35DA5E1C708BD27375B30_AdjustorThunk','_NativeArray_1_Equals_mE6D370116FDE0140B368F32A5ABA362C787390FD_AdjustorThunk','_NativeArray_1_Equals_mB93BCE5B37BF99DAD0F42C77B469C5058D7082B3_AdjustorThunk','_NativeArray_1_Equals_m2B2ABB1220BB23B001EF8ECCC8716CB19CFB9F66_AdjustorThunk','_NativeArray_1_Equals_m7923EAFE69C4811E2802FB5DAEE26DB0ACDA5848_AdjustorThunk','_NativeArray_1_Equals_mAB8CD253CB736D087389744F61DB461C28AF2A90_AdjustorThunk','_NativeArray_1_Equals_m28EE88C53C8CCEF40EAB50C7BB5989101DB1DC7C_AdjustorThunk','_NativeArray_1_Equals_m0AE8EDDC59401CB04779CC9AD109ABE8112DDAF3_AdjustorThunk','_NativeArray_1_Equals_m658A996A61D91F4626659A0F0E7006685DC21011_AdjustorThunk','_NativeArray_1_Equals_m1D59FD3D75A56F8BAB17D309A22A962DE1563992_AdjustorThunk','_NativeArray_1_Equals_mE0F0C41D4F2A1455C439C6616849A62B25BA18F9_AdjustorThunk','_NativeArray_1_Equals_m027DCC5AF6A069A6B3E875A67B2471261F6BC6AC_AdjustorThunk','_NativeArray_1_Equals_mFA9A6A0C999E5D18918DECBDC16C7C03AF7F75E5_AdjustorThunk','_NativeArray_1_Equals_m8F9C07551B9664040D77DDD105D66A24E474E969_AdjustorThunk','_NativeArray_1_Equals_mA605491D03C6724D66656DABF63AA0CCFF5345AE_AdjustorThunk','_NativeArray_1_Equals_mD40B7B4AF274911B0C60BDD004861055A25178EE_AdjustorThunk','_NativeArray_1_Equals_mB82BBA7E4F83D9C63140620A74D23267D7791C38_AdjustorThunk','_NativeArray_1_Equals_mF0AB163CD1A991CCBB04782D27EF4AE45F1D448D_AdjustorThunk','_NativeArray_1_Equals_m1147DA88E9FB1832E8F39CBDC6A78D1613404257_AdjustorThunk','_NativeArray_1_Equals_mCE8A146736E9714620003C9FDABED0EB1D9ED3B6_AdjustorThunk','_NativeArray_1_Equals_m4A735EC55B7D446F7C62F4BB22396268B234E7D3_AdjustorThunk','_NativeArray_1_Equals_m85367A1332483FEBC192797BB6A773A2935BAD20_AdjustorThunk','_NativeArray_1_Equals_m517137176B5D08881E17291B80AF84F66A2EED29_AdjustorThunk','_NativeArray_1_Equals_m39770B88695DAB34A51F2DB40DA460F5EC76CB3F_AdjustorThunk','_NativeArray_1_Equals_m36866073359E4373E7DA6E6C7931C8A88E4828EB_AdjustorThunk','_NativeArray_1_Equals_m7C379A2D38AA91C437BECE33D0C2D7459A33F60B_AdjustorThunk','_NativeArray_1_Equals_m0DBEBFDE1E6EACA27DFA058EAF10791A155E5A0A_AdjustorThunk','_NativeArray_1_Equals_m4703026056C4C0DBDFE3BC7268D14FA66A5FF0F0_AdjustorThunk','_NativeArray_1_Equals_mA6721EF9497AAA65A695C81D8001A59204EB9158_AdjustorThunk','_NativeArray_1_Equals_m6F545B4034662E57408647560175D0306245030D_AdjustorThunk','_NativeSlice_1_Equals_m3B497EE67C514347FDABE033886F616E0269C727_AdjustorThunk','_NativeSlice_1_Equals_m477F05EC50689DE914C61D7A93DB16696C8828F6_AdjustorThunk','_BlobAssetReference_1_Equals_mE3BCC6F0F05ACBF6869568412E02B9BB330DB275_AdjustorThunk','_BlobAssetReference_1_Equals_mE6D50BD388F7732D3A499581F5FFFAD4071B9948_AdjustorThunk','_BlobAssetReference_1_Equals_mDDC62B46E4CD92841966C6C33BDF143B8D0D6253_AdjustorThunk','_BlobAssetReference_1_Equals_m9498A6DC29605C89B25DFCCD011B4B5A59A0F96B_AdjustorThunk','_BlobAssetReference_1_Equals_m7AEAF0782B3895E1351BEE580B54C1C6301AA467_AdjustorThunk','_BlobAssetReference_1_Equals_mB28C8B6290A344704AEEDDE3B2C5112F081D42F3_AdjustorThunk','_BlobAssetReference_1_Equals_mABDBDA392EB844DC69C334CEB200B4D448ACACD3_AdjustorThunk','_BlobAssetReference_1_Equals_m724E86BC0E6ABADBDA084C4EBAD71D6B3730B9F4_AdjustorThunk','__ZN4bgfx2gl17RendererContextGL11getInternalENS_13TextureHandleE','__ZN2bx17StaticMemoryBlock4moreEj','__ZNKSt3__219__shared_weak_count13__get_deleterERKSt9type_info','__ZN4bgfx4noop19RendererContextNOOP11getInternalENS_13TextureHandleE','_U3CU3Ec_U3CSortSystemUpdateListU3Eb__7_0_mFDF703D97C4034CE40098A8C9D55FBCB3AA8A2E4','__ZN4bgfxL17compareDescendingEPKvS1_','__ZZN7tinystl4listIN4bgfx17NonLocalAllocator4FreeENS1_16TinyStlAllocatorEE4sortEvENUlPKvS7_E_8__invokeES7_S7_','_emscripten_glGetAttribLocation','_emscripten_glGetUniformLocation','_emscripten_glGetFragDataLocation','_emscripten_glGetStringi','_emscripten_glGetUniformBlockIndex','_emscripten_glFenceSync',0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0];
var debug_table_iiii = [0,'_Int32_ToString_m6B210A3563C22C0640F05004791AFFDAF9D715A1_AdjustorThunk','_Double_ToString_mB1A3F7A4412911158D222E8255D5CEA28C9B7151_AdjustorThunk','_UInt32_ToString_mFAA119806993132F73BB2977F26E129A2D09D66D_AdjustorThunk','_UInt64_ToString_m1F7EDB4BAE7C1F734ECA643B3F3FA8350237A60A_AdjustorThunk','_Guid_ToString_mA9FF4461B4210034B6F9F7420F1B38EA63D3319C_AdjustorThunk','_SByte_ToString_m5E4FEAA7BD60F4D7C2797935C7337166579AB290_AdjustorThunk','_Byte_ToString_m1354398A7B093824D78D4AB1D79A6B6C304DB054_AdjustorThunk','_Int16_ToString_mB8D1A605787E6CBF8D1633314DAD23662261B1F1_AdjustorThunk','_UInt16_ToString_m03559E4ED181D087816EBBFAB71BCD74369EDB4F_AdjustorThunk','_Int64_ToString_m23550A17C2F7CBE34140C902C8E67A8A46FC47DD_AdjustorThunk','_Single_ToString_mC457A7A0DAE1E2E73182C314E22D6C23B01708CA_AdjustorThunk','_float2_ToString_mD74D65464FCFD636D20E1CF9EE66FBF8FBF106C7_AdjustorThunk','_float4_ToString_mD78689CF846A1F9500B643457B44F2621469FF51_AdjustorThunk','_float3_ToString_mBB1AE2BEB31583E1D5F84C3157A4227BBDB4378E_AdjustorThunk','_float3x3_ToString_m6603D72B66AC77FA88CE954E8B2424983F87EBCC_AdjustorThunk','_uint3_ToString_m03B57D27B3ECF16EB5304F14BED619D9E25A48AB_AdjustorThunk','_float4x4_ToString_mE9625D0939639D1BDF58292F4D3A679677A753F5_AdjustorThunk','_uint4_ToString_mC2E1F3FC7E97C5FC44259E3D3D2F3AB226E85528_AdjustorThunk','_uint2_ToString_m8B303780379D9A634CEE11E0C262F6A7C552C862_AdjustorThunk','_quaternion_ToString_m61124B348E7E089461C6DEED0E01D1F8D8347408_AdjustorThunk','_BasicComparer_1_Equals_m2D0CF4969843E032504180F6BD4C9E49E1B44A27','_BasicComparer_1_Equals_m68FAE6F4081667D55A1168E1A1778FC43AF736E3','_BasicComparer_1_Equals_mFB01E8C6BFFF172537CBE4883D3D08CADB0A36C9','_BasicComparer_1_Equals_mBE92B34ECD1DD7C4DE81056FE39478183747D74C','_BasicComparer_1_Equals_mB14F4F3BC435E37CC035F6D75F14E710DC0C8DBA','_BasicComparer_1_Equals_m99C718659DC3EA8C24C8B0C8C23B4B4E9B99B921','_BasicComparer_1_Equals_m6C966D174CA47B716E33B6CA90E35C2F464234E2','___stdio_write','___stdio_seek','___stdout_write','_sn_write','__ZNK10__cxxabiv117__class_type_info9can_catchEPKNS_16__shim_type_infoERPv','_StructuralChange_AddComponentEntityExecute_mBD6CF6E0BD569C38B5D553AF6E1732C1A821C0CC','_StructuralChange_RemoveComponentEntityExecute_mCCDA16C549F039B003EA0378D31386228F3B0A8D','___stdio_read',0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0];
var debug_table_iiiii = [0,'_AddComponentEntityDelegate_Invoke_mE45126207FEE7AC9FD3CAFF564B88E5090FF969F','_RemoveComponentEntityDelegate_Invoke_m78734E30747ECD8B12BA08B73EB32EC2FEB9719B','__ZN2bx10FileWriter4openERKNS_8FilePathEbPNS_5ErrorE','__ZN2bx10FileWriter5writeEPKviPNS_5ErrorE','__ZThn8_N2bx10FileWriter5writeEPKviPNS_5ErrorE','__ZN2bx12MemoryWriter5writeEPKviPNS_5ErrorE','__ZN2bx11SizerWriter5writeEPKviPNS_5ErrorE','__ZN2bx12MemoryReader4readEPviPNS_5ErrorE','__ZN4bgfx2gl10LineReader4readEPviPN2bx5ErrorE','__ZN2bx14FileWriterImpl4openERKNS_8FilePathEbPNS_5ErrorE','__ZN2bx14FileWriterImpl5writeEPKviPNS_5ErrorE','__ZThn8_N2bx14FileWriterImpl5writeEPKviPNS_5ErrorE','_GC_gcj_fake_mark_proc','_emscripten_glMapBufferRange',0];
var debug_table_iiiiiii = [0,'__ZN2bx16DefaultAllocator7reallocEPvmmPKcj','__ZN4bgfx13AllocatorStub7reallocEPvmmPKcj','__ZN4bgfx12AllocatorC997reallocEPvmmPKcj'];
var debug_table_iiiiiiiii = [0,'_Image2DIOHTMLLoader_CheckLoading_mD838C25F912B3BCCA8EF26439356AAA6B7C6E0C2','_AudioHTMLSystemLoadFromFile_CheckLoading_m82121B914F56FA672C2181E610B5899D17C0320F',0];
var debug_table_iiiiiiiiiiii = [0];
var debug_table_iiiiiiiiiiiii = [0];
var debug_table_iiiiji = [0,'__ZN4bgfx2gl17RendererContextGL13createTextureENS_13TextureHandleEPKNS_6MemoryEyh','__ZN4bgfx4noop19RendererContextNOOP13createTextureENS_13TextureHandleEPKNS_6MemoryEyh',0];
var debug_table_iiij = [0,'_emscripten_glClientWaitSync'];
var debug_table_iij = [0,'_UInt64_Equals_m69503C64A31D810966A48A15B5590445CA656532_AdjustorThunk','_UInt64_CompareTo_m9546DD4867E661D09BB85FDED17273831C4B96E2_AdjustorThunk','_Int64_Equals_mA5B142A6012F990FB0B5AA144AAEB970C26D874D_AdjustorThunk','_Int64_CompareTo_m7AF08BD96E4DE2683FF9ED8DF8357CA69DEB3425_AdjustorThunk','__ZN4bgfx12CallbackStub13cacheReadSizeEy','__ZN4bgfx11CallbackC9913cacheReadSizeEy','__ZL15cache_read_sizeP25bgfx_callback_interface_sy'];
var debug_table_iijii = [0,'__ZN4bgfx12CallbackStub9cacheReadEyPvj','__ZN4bgfx11CallbackC999cacheReadEyPvj','__ZL10cache_readP25bgfx_callback_interface_syPvj'];
var debug_table_ji = [0,'_Enumerator_get_Current_m95C1EF83AC550AF880BF1B88DA413BBF613E3A2C_AdjustorThunk'];
var debug_table_jiji = [0,'__ZN2bx10FileWriter4seekExNS_6Whence4EnumE','__ZThn12_N2bx10FileWriter4seekExNS_6Whence4EnumE','__ZN2bx12MemoryWriter4seekExNS_6Whence4EnumE','__ZThn4_N2bx12MemoryWriter4seekExNS_6Whence4EnumE','__ZN2bx11SizerWriter4seekExNS_6Whence4EnumE','__ZThn4_N2bx11SizerWriter4seekExNS_6Whence4EnumE','__ZN2bx12MemoryReader4seekExNS_6Whence4EnumE','__ZThn4_N2bx12MemoryReader4seekExNS_6Whence4EnumE','__ZN2bx14FileWriterImpl4seekExNS_6Whence4EnumE','__ZThn12_N2bx14FileWriterImpl4seekExNS_6Whence4EnumE',0,0,0,0,0];
var debug_table_v = [0,'__ZN4bgfx4noop15rendererDestroyEv','__ZN4bgfx4d3d915rendererDestroyEv','__ZN4bgfx5d3d1115rendererDestroyEv','__ZN4bgfx5d3d1215rendererDestroyEv','__ZN4bgfx3gnm15rendererDestroyEv','__ZN4bgfx3nvn15rendererDestroyEv','__ZN4bgfx2gl15rendererDestroyEv','__ZN4bgfx2vk15rendererDestroyEv','__ZL25default_terminate_handlerv','_ReversePInvokeWrapper_U3CU3Ec__DisplayClass0_0_U3CMainU3Eb__0_m38308E5629152C6F37DDB1F8B7C2F30141860823','_ReversePInvokeWrapper_CarAudioSystem_U3COnUpdateU3Eb__0_0_mFB69A0075371C0CAAB141894C9F4920FED505AD2','_ReversePInvokeWrapper_CarAudioSystem_U3COnUpdateU3Eb__0_1_mABDDCDE9A75D1E5CCF8914AE14B07C3E0596F9BF','_ReversePInvokeWrapper_CarAudioSystem_U3COnUpdateU3Eb__0_2_mE42A5B66C31335991A29D4CB4D46238772F07672','_ReversePInvokeWrapper_U3CU3Ec__DisplayClass0_0_U3COnUpdateU3Eb__3_mDF9D34EE06951AE1E5EC5BCD084B01EEAFF1D9ED','_ReversePInvokeWrapper_EnterBoostPad_U3COnUpdateU3Eb__0_0_m106E5409421E88409B63576E336CD430995B4819','_ReversePInvokeWrapper_U3CU3Ec__DisplayClass0_0_U3COnUpdateU3Eb__1_mAE9B89998F50B4380E3789D20BA35C71C48EA4BE','_ReversePInvokeWrapper_U3CU3Ec__DisplayClass5_0_U3COnUpdateU3Eb__0_m75B9983438829AB16C524E9A6775875E6A2F6DC6','_ReversePInvokeWrapper_U3CU3Ec_U3COnUpdateU3Eb__5_1_m7FE245C8713D9B0F6A79A60D81CDFFD49939C3B1','_ReversePInvokeWrapper_U3CU3Ec_U3COnUpdateU3Eb__5_2_m531AAF4CC48E3F24402760144F30335B4210A243','_ReversePInvokeWrapper_U3CU3Ec__DisplayClass2_0_U3COnUpdateU3Eb__0_mA5CB008664E7B83171D712154C4777C1F35C6D60','_ReversePInvokeWrapper_U3CU3Ec__DisplayClass5_0_U3COnUpdateU3Eb__0_mA14AAE3C0D2628931866E561DB8C6ECB4F3E8DA6','_ReversePInvokeWrapper_U3CU3Ec__DisplayClass5_0_U3COnUpdateU3Eb__1_m9C1F3E49E5988926D3D10906335D54F400A49B53','_ReversePInvokeWrapper_U3CU3Ec__DisplayClass0_0_U3COnUpdateU3Eb__0_m949AE7ABC6A8D9433C9B8F5832DE0FBBEB01A103','_ReversePInvokeWrapper_U3CU3Ec__DisplayClass1_0_U3COnUpdateU3Eb__0_mFDE94DEC5FA8E02ABB8A522F78455C8BFCC2E487','_ReversePInvokeWrapper_U3CU3Ec__DisplayClass1_0_U3COnUpdateU3Eb__1_mE2EB8CFB82D2D331239F72E8BA7A841EAC28DD23','_ReversePInvokeWrapper_U3CU3Ec__DisplayClass1_0_U3COnUpdateU3Eb__0_m51821E665506FDD249D33A3EC2162D84B8AB16E6','_ReversePInvokeWrapper_UpdateGameOverMenu_U3CSetMenuVisibilityU3Eb__2_0_m2DAD37E78B92ACB6B10CB44A28E1819589528330','_ReversePInvokeWrapper_UpdateGameOverMenu_U3CSetMenuVisibilityU3Eb__2_1_m5664EA053E81D91F8B945910295CA6FA0AA35A99','_ReversePInvokeWrapper_UpdateGameOverMenu_U3CSetMenuVisibilityU3Eb__2_2_m2EE13033885A09E1451C82F6A2ADC2953811F16A','_ReversePInvokeWrapper_U3CU3Ec__DisplayClass1_0_U3COnUpdateU3Eb__0_m8C22C338C15AA4F130672BE19BC07DC2B3968B5D','_ReversePInvokeWrapper_U3CU3Ec__DisplayClass1_0_U3COnUpdateU3Eb__1_mB2D3FE214B507CA955CD0BCA9DBDF4BA2458BBFC','_ReversePInvokeWrapper_U3CU3Ec__DisplayClass1_0_U3COnUpdateU3Eb__2_m90E2467BA0CAE2E6CBD42B7C556B22F9BF6571F3','_ReversePInvokeWrapper_U3CU3Ec__DisplayClass1_0_U3COnUpdateU3Eb__3_mD0767017CE62795DC4639378F4E386BF7F02A06C','_ReversePInvokeWrapper_U3CU3Ec__DisplayClass1_0_U3COnUpdateU3Eb__4_m0AA1DA1FF6B5B979D14CFECBA50A6A6CA3D8D01B','_ReversePInvokeWrapper_U3CU3Ec__DisplayClass1_0_U3COnUpdateU3Eb__5_mF4DF40B6149E1B1EC774D706657B17D5DCC51E18','_ReversePInvokeWrapper_U3CU3Ec__DisplayClass1_0_U3COnUpdateU3Eb__6_m8919A75A65A2BCE1B5BE97415274B949A4E3D9FE','_ReversePInvokeWrapper_U3CU3Ec__DisplayClass1_0_U3COnUpdateU3Eb__7_mE5AB092B246F2CBAE1FE32D6DFA3B6F697E386E9','_ReversePInvokeWrapper_UpdateGameplayMenu_U3CSetMenuVisibilityU3Eb__2_0_m1E4513FDE3230F6B0159A71533D107C779E77FBE','_ReversePInvokeWrapper_UpdateGameplayMenu_U3CSetMenuVisibilityU3Eb__2_1_m480617EE1C341E7C9D71DBBEDF32922DC16D1B3D','_ReversePInvokeWrapper_UpdateGameplayMenu_U3CSetMenuVisibilityU3Eb__2_2_mF9FFBC52E7115BA297F1FCB71F5D2EC2E0DC64C6','_ReversePInvokeWrapper_UpdateGameplayMenu_U3CSetMenuVisibilityU3Eb__2_3_mB8DBA73571BAEA4D7AC66864BFC8353753289ED5','_ReversePInvokeWrapper_UpdateGameplayMenu_U3CSetMenuVisibilityU3Eb__2_4_m1C2019F7A9411962D8B024C74B887CA3FD53A5E4','_ReversePInvokeWrapper_UpdateMainMenu_U3CSetMenuVisibilityU3Eb__2_0_mE2A867DFA8EEFDAEE9343225605DA12693319B89','_ReversePInvokeWrapper_UpdateMainMenu_U3CSetMenuVisibilityU3Eb__2_1_mF739DC1D8D65278ED160A8A48908ED7E79AE9E3A','_ReversePInvokeWrapper_UpdateMainMenu_U3CSetMenuVisibilityU3Eb__2_2_m47F5213811689A81390DDC71CD9ACD0FFFEFEAB1','_ReversePInvokeWrapper_U3CU3Ec__DisplayClass3_0_U3COnUpdateU3Eb__0_m3A79D4C4E0854BDB3B0AE25A4B0F27BA4CF31AE7','_ReversePInvokeWrapper_U3CU3Ec__DisplayClass5_1_U3COnUpdateU3Eb__4_m14A88E2CFD9B02C824A4B6D7647D4CD2807FC937','_ReversePInvokeWrapper_U3CU3Ec__DisplayClass5_0_U3COnUpdateU3Eb__5_mEEB709AB5BA673E478A67D43556814EE6B18F27B','_ReversePInvokeWrapper_U3CU3Ec__DisplayClass5_0_U3COnUpdateU3Eb__0_mF863D8FFFDD5DAB07A3D02D7A935F4996EE792BF','_ReversePInvokeWrapper_U3CU3Ec__DisplayClass5_0_U3COnUpdateU3Eb__1_m6AFBBA09FF87995493C532C34E7D4762F940BB67','_ReversePInvokeWrapper_U3CU3Ec__DisplayClass5_0_U3COnUpdateU3Eb__2_m8E1BEE4AEF9F95AD709F3B17EA1EAFF5ABD16BC6','_ReversePInvokeWrapper_U3CU3Ec__DisplayClass5_0_U3COnUpdateU3Eb__3_mBF38E2B541EE38275DF76CC02BFC53D807B6B7A0','_ReversePInvokeWrapper_U3CU3Ec_U3COnUpdateU3Eb__1_0_m9478441D6C1037A723A1A0EB6001CE4B38025BE5','_ReversePInvokeWrapper_U3CU3Ec_U3COnUpdateU3Eb__1_1_mAD7781674849ED6B84EAE1D3AF30F6E25E3B6572','_ReversePInvokeWrapper_U3CU3Ec__DisplayClass1_0_U3COnUpdateU3Eb__2_m18EEE8252E3429D6774CB1E88DEEA4D39993DC09','_ReversePInvokeWrapper_U3CU3Ec__DisplayClass1_0_U3COnUpdateU3Eb__3_mE150762AB6558CEF7143CD9C3C279572E014BFEB','_ReversePInvokeWrapper_U3CU3Ec__DisplayClass1_0_U3COnUpdateU3Eb__4_mB12BD304A1A5DE24317695F1F5B56AE9BD55A083','_ReversePInvokeWrapper_U3CU3Ec__DisplayClass1_0_U3COnUpdateU3Eb__5_mFF3E0D8F621DDF9FEEBA0E27C1923386F03FA568','_ReversePInvokeWrapper_U3CU3Ec__DisplayClass1_0_U3COnUpdateU3Eb__6_m813BBA163868A33B4DAE8539FDFCC5D52B31C23B','_ReversePInvokeWrapper_U3CU3Ec__DisplayClass10_0_U3CBuildDefaultRenderGraphU3Eb__0_m80D5EF2146B78BE359B2326D8088910872963A22','_ReversePInvokeWrapper_U3CU3Ec__DisplayClass14_0_U3CBuildAllLightNodesU3Eb__0_mBDDF6D193B05B2A14136AFC168D836481D44B2B4','_ReversePInvokeWrapper_U3CU3Ec__DisplayClass95_0_U3CReloadAllImagesU3Eb__0_m34E04B286B17711E8ABB8DBDCD4CCED40673F1BE','_ReversePInvokeWrapper_U3CU3Ec__DisplayClass96_0_U3CDestroyAllTexturesU3Eb__0_mB59224FA6C05FEDF336E705B9F9723653B145321','_ReversePInvokeWrapper_U3CU3Ec__DisplayClass96_0_U3CDestroyAllTexturesU3Eb__1_m92D29E97B028AD3EFCCE3D1E9B66BE57458227DC','_ReversePInvokeWrapper_U3CU3Ec__DisplayClass96_0_U3CDestroyAllTexturesU3Eb__2_m8DDD63AF4B074F49BE9AE6700BE73A791FD61363','_ReversePInvokeWrapper_U3CU3Ec__DisplayClass96_0_U3CDestroyAllTexturesU3Eb__3_m6131FF7A9C9D31467614F6FECC9778A0917103CC','_ReversePInvokeWrapper_U3CU3Ec__DisplayClass97_0_U3CShutdownU3Eb__0_m8DF3D9A115D50EE87DA0245E488F44FF02D1A289','_ReversePInvokeWrapper_U3CU3Ec__DisplayClass97_0_U3CShutdownU3Eb__1_mD37FDEDA74960AE455722E4E803BBAAB5EB55447','_ReversePInvokeWrapper_U3CU3Ec__DisplayClass97_0_U3CShutdownU3Eb__2_mD7B477F32BBD4C81E8A87FEE38037E8B950541CC','_ReversePInvokeWrapper_U3CU3Ec_U3CUpdateExternalTexturesU3Eb__123_0_m7ADB410BA28D55D28D781500F6A1430674BA004B','_ReversePInvokeWrapper_U3CU3Ec__DisplayClass127_0_U3CUploadTexturesU3Eb__0_m478525F5070B2CC99401EE3DF1205E50DE55B5D7','_ReversePInvokeWrapper_U3CU3Ec__DisplayClass127_0_U3CUploadTexturesU3Eb__1_mB9A4FAEB7F9C6054B8F9C816B8742487AE3D112C','_ReversePInvokeWrapper_U3CU3Ec__DisplayClass120_0_U3CUploadMeshesU3Eb__0_m5AA8ADF6FF638D80360588FC08723670EAED4B34','_ReversePInvokeWrapper_U3CU3Ec__DisplayClass120_0_U3CUploadMeshesU3Eb__1_m50A47F69557D1F05655D8C08439B9DB60ADAF5C7','_ReversePInvokeWrapper_U3CU3Ec__DisplayClass128_0_U3CUpdateRTTU3Eb__0_m3D06490282ECAB45A4FBD412C5467CEA8FB3F194','_ReversePInvokeWrapper_U3CU3Ec__DisplayClass128_0_U3CUpdateRTTU3Eb__1_mE12234CDCA382F97D79BA995792FC83C487D363E','_ReversePInvokeWrapper_U3CU3Ec__DisplayClass0_0_U3COnUpdateU3Eb__0_m776C73440E442A1E63B17752A6302A00D7E1DA0E','_ReversePInvokeWrapper_U3CU3Ec__DisplayClass5_0_U3COnUpdateU3Eb__0_mCE366DDA320ECCD52497358AB7D99F83E895C3F5','_ReversePInvokeWrapper_U3CU3Ec__DisplayClass5_0_U3COnUpdateU3Eb__1_m5747F8EF3CEC2B1E158471F74DEDCB21BD955F64','_ReversePInvokeWrapper_U3CU3Ec__DisplayClass5_0_U3COnUpdateU3Eb__2_mAADD3C5204127D78474F870A2AF12D589DFBD12A','_ReversePInvokeWrapper_U3CU3Ec__DisplayClass5_0_U3COnUpdateU3Eb__3_m1A53D70701E6BE7150A49AD758628B4B405F4D93','_ReversePInvokeWrapper_U3CU3Ec__DisplayClass5_0_U3COnUpdateU3Eb__4_mA939170879434BA7CD8C53F20D4DDF3FB349BE3B','_ReversePInvokeWrapper_U3CU3Ec__DisplayClass5_0_U3COnUpdateU3Eb__5_mD8A6A397139516F8CA68886DD5A7DE16AEFF3D2F','_ReversePInvokeWrapper_U3CU3Ec__DisplayClass5_0_U3COnUpdateU3Eb__6_mB72E0E4C2713BAB44737281573DDA3D4F0D6833E','_ReversePInvokeWrapper_U3CU3Ec__DisplayClass0_0_U3COnUpdateU3Eb__0_mF357A248E8A2A39BF4832337A4D7AF2922F3117D','_ReversePInvokeWrapper_U3CU3Ec__DisplayClass1_0_U3COnUpdateU3Eb__0_mE433E5CA0D11A03A97969C43D129B0AC13204991','_ReversePInvokeWrapper_U3CU3Ec__DisplayClass1_0_U3COnUpdateU3Eb__1_m8667FFB66CEAA16D3026D8F547F62844666E1C20','_ReversePInvokeWrapper_U3CU3Ec__DisplayClass1_0_U3COnUpdateU3Eb__2_m6CC0AF223F4D76CFAE8FB3CC3E2543465B7E13AA','_ReversePInvokeWrapper_U3CU3Ec__DisplayClass1_0_U3COnUpdateU3Eb__3_m3C9B9F06D05BEB8EA98D108AEF958705F916E498','_ReversePInvokeWrapper_U3CU3Ec__DisplayClass1_0_U3COnUpdateU3Eb__4_mD9DF99810A1A6FA735BCBCC948B2215EF216B088','_ReversePInvokeWrapper_U3CU3Ec__DisplayClass1_0_U3COnUpdateU3Eb__5_m91051E1E0861E56C31FAAA7197C68563AC514076','_ReversePInvokeWrapper_U3CU3Ec_U3COnUpdateU3Eb__1_6_m0ADF5549955FFDBCA34483854B9B1AF9A81C3C1E','_ReversePInvokeWrapper_U3CU3Ec__DisplayClass5_0_U3COnUpdateU3Eb__0_mBB945BF042B0B8142B48C98784E96B6CECDDF05A','_ReversePInvokeWrapper_U3CU3Ec__DisplayClass5_0_U3COnUpdateU3Eb__1_m07BCF222F9E4856B4B5760A718B36F6CC4360C74','_ReversePInvokeWrapper_U3CU3Ec__DisplayClass5_0_U3COnUpdateU3Eb__2_m20CEF4A43E3C8A76A394F843BECB40A5518DBC69','_ReversePInvokeWrapper_U3CU3Ec__DisplayClass5_0_U3COnUpdateU3Eb__3_mF7A5234B57925D66BC1912AC5A0A1A1F28F298C3','_ReversePInvokeWrapper_U3CU3Ec__DisplayClass5_0_U3COnUpdateU3Eb__4_mA2EA38AB4C7F86D805C722F57681CE2756094290','_ReversePInvokeWrapper_U3CU3Ec__DisplayClass5_0_U3COnUpdateU3Eb__5_mE11458B12BBAABA0C51C014D5D35E6245FD7812C','_ReversePInvokeWrapper_U3CU3Ec__DisplayClass5_0_U3COnUpdateU3Eb__6_mD1ED596DEA3F60ADA9FCE9CA35333A0310FA9ED5','_ReversePInvokeWrapper_U3CU3Ec_U3CSortSystemUpdateListU3Eb__7_0_mFDF703D97C4034CE40098A8C9D55FBCB3AA8A2E4','_ReversePInvokeWrapper_StructuralChange_AddComponentEntitiesBatchExecute_mA9992EAFAB17A435D35C09B990AE5FAE52676A39','_ReversePInvokeWrapper_StructuralChange_AddComponentEntityExecute_mBD6CF6E0BD569C38B5D553AF6E1732C1A821C0CC','_ReversePInvokeWrapper_StructuralChange_AddComponentChunksExecute_m93FADB4248E9D744F87C5BA0A92F6D85F9C87720','_ReversePInvokeWrapper_StructuralChange_RemoveComponentEntityExecute_mCCDA16C549F039B003EA0378D31386228F3B0A8D','_ReversePInvokeWrapper_StructuralChange_RemoveComponentEntitiesBatchExecute_m6632C5213792F71C74F594B1A5FE346C95533033','_ReversePInvokeWrapper_StructuralChange_RemoveComponentChunksExecute_m884C1F67D3E5366A235EFFF73BECAD43451251AE','_ReversePInvokeWrapper_StructuralChange_AddSharedComponentChunksExecute_mDE42CA5BEB4AA2BD8D338F87AAE78260366C4C69','_ReversePInvokeWrapper_StructuralChange_MoveEntityArchetypeExecute_m1FEF3D40A2CDF4B15AAF65BA953B04EADA5F5628','_ReversePInvokeWrapper_StructuralChange_SetChunkComponentExecute_m2C93664388AEC82B9530D7B83D4A5D30BA04AB90','_ReversePInvokeWrapper_StructuralChange_CreateEntityExecute_m004B3E705017E2710FF182143178D852D16D08AB','_ReversePInvokeWrapper_StructuralChange_InstantiateEntitiesExecute_mCC1E269F8C1720814E7F240E61D755E9E7B4AE5F','_ReversePInvokeWrapper_U3CU3Ec__DisplayClass3_0_U3CInitializeSystemsU3Eb__0_m9719A5FE728EDE1FBF0C72105AC8544447F5CBED','_ReversePInvokeWrapper_U3CU3Ec__DisplayClass3_0_U3CInitializeSystemsU3Eb__1_mF7CB925DD32BC2BD91BE2D76B4C5CB886FB40C07','_ReversePInvokeWrapper_U3CU3Ec__DisplayClass_OnUpdate_LambdaJob0_PerformLambda_m87BE33CFD398760E10F74AFEFE10EF352F280A46','_ReversePInvokeWrapper_U3CU3Ec__DisplayClass_OnUpdate_LambdaJob1_PerformLambda_mBE1855D34FA165EEBA9634C3F05A62C93A52382C','_ReversePInvokeWrapper_U3CU3Ec__DisplayClass_OnUpdate_LambdaJob2_PerformLambda_m847B8710686A7AEBC61CECB1A7FC11F3475F04C2','_ReversePInvokeWrapper_U3CU3Ec__DisplayClass1_0_U3COnUpdateU3Eb__0_mA39B449C7A2078637A42B949E02955ED9CD428AD','_ReversePInvokeWrapper_U3CU3Ec__DisplayClass4_0_U3CFindCameraU3Eb__0_m27D9987C1502F10E5287A96C6223C8785DAFFE4A','_ReversePInvokeWrapper_U3CU3Ec__DisplayClass4_0_U3CFindCameraU3Eb__1_m22EB15E590A8B5F55AEF94C4F0F08EF649CC2812','_ReversePInvokeWrapper_U3CU3Ec__DisplayClass5_0_U3COnUpdateU3Eb__0_mF493768363F5D07FC825887ACE82E7B87242BFE7','_ReversePInvokeWrapper_U3CU3Ec__DisplayClass1_0_U3COnUpdateU3Eb__0_m7E49CE549BBA2FE2BC5E820ADE602F8290C9492E','_ReversePInvokeWrapper_U3CU3Ec__DisplayClass1_1_U3COnUpdateU3Eb__2_mD57FDB20953DDB0A156660F2A364DDD8543EC1E6','_ReversePInvokeWrapper_U3CU3Ec__DisplayClass1_0_U3COnUpdateU3Eb__1_m07F088155110352443891FB846561D682308D5B4','_ReversePInvokeWrapper_U3CU3Ec__DisplayClass2_0_U3COnUpdateU3Eb__0_m91062E044ED0E6966C9DE2EF173BA0904BDEF5DE','_ReversePInvokeWrapper_U3CU3Ec__DisplayClass2_0_U3COnUpdateU3Eb__1_mB408CC63D9C37D30E5A53EA6677A38E5CC853450','_ReversePInvokeWrapper_U3CU3Ec_U3COnUpdateU3Eb__2_2_m7321023A1B663304F2E2CF7968DC40BCF503C8DE','_ReversePInvokeWrapper_U3CU3Ec_U3COnUpdateU3Eb__2_3_m44FD77C0F2F0CF7F99DB1A55C4AC0C1ECD1D6CFB','_ReversePInvokeWrapper_UpdateLightMatricesSystem_U3COnUpdateU3Eb__0_0_m2E333E0AF243F78EBB124B1581B092DEDFD0C7B9','_ReversePInvokeWrapper_UpdateLightMatricesSystem_U3COnUpdateU3Eb__0_1_m6D7A2B75C69EBD63B8655429FDB913D0F1945945','_ReversePInvokeWrapper_U3CU3Ec_U3COnUpdateU3Eb__0_2_mCA0DD9776DD5875F81412F69F1F8719221D1D208','_ReversePInvokeWrapper_U3CU3Ec_U3COnUpdateU3Eb__0_3_m2BCED6195898404A128CBB15665FEB93A7E719F0','_ReversePInvokeWrapper_U3CU3Ec_U3COnUpdateU3Eb__0_4_m80C9EA9FC0FA6DDA241A2557DD169963016C8D40','_ReversePInvokeWrapper_U3CU3Ec__DisplayClass0_0_U3COnUpdateU3Eb__0_m0E8BC2527CC3597126CEB818E8A1FE98B8D9CFBA','_ReversePInvokeWrapper_U3CU3Ec__DisplayClass0_0_U3COnUpdateU3Eb__1_m48A22216FA0435EE5098FDBDEB682E6011ED828C','_ReversePInvokeWrapper_U3CU3Ec__DisplayClass0_0_U3COnUpdateU3Eb__2_m3BD60A1F0BD821A262CF6FFE30BF0E6A7D5CC8AF','_ReversePInvokeWrapper_U3CU3Ec__DisplayClass0_0_U3COnUpdateU3Eb__3_m9064FC96520027D26E73C557781B5E2E1FD4006E','_ReversePInvokeWrapper_U3CU3Ec__DisplayClass0_0_U3COnUpdateU3Eb__4_m7520874AD084443E8CCD4962D6F25197C3BA2B10','_ReversePInvokeWrapper_U3CU3Ec_U3COnUpdateU3Eb__0_5_m65E29A5FC31C1262B4523022C0A87B933FC5279E','_ReversePInvokeWrapper_U3CU3Ec_U3COnUpdateU3Eb__0_6_m636627C8FDE65C5D7321489EC2571728F27FF4EA','_ReversePInvokeWrapper_U3CU3Ec_U3COnUpdateU3Eb__0_7_mB57412808EA7509A60FB1AFB9D6B83FFAC77135D','_ReversePInvokeWrapper_AudioSystem_U3COnUpdateU3Eb__7_0_m0D5C2BBA8FAB6E90D2AEBA6488BB0CDE21B37074','_ReversePInvokeWrapper_AudioSystem_U3COnUpdateU3Eb__7_1_mB3A33DD40BC8367BB86E91058B3A3C2E0821DFFC','_ReversePInvokeWrapper_AudioSystem_U3COnUpdateU3Eb__7_2_m625D733B21AF3CDB4D2F660948681FBE960AFB88','_ReversePInvokeWrapper_AudioSystem_U3COnUpdateU3Eb__7_3_mBAAD9D1912ECADF15D54A08A3AD779A615EEDDCE','_ReversePInvokeWrapper_U3CU3Ec__DisplayClass7_0_U3COnUpdateU3Eb__0_m69465EA8081E657462A5E571D4B1026C1193F346','__ZN4bgfx2glL17stubPopDebugGroupEv','_emscripten_glFinish','_emscripten_glFlush','_emscripten_glReleaseShaderCompiler','_emscripten_glEndTransformFeedback','_emscripten_glPauseTransformFeedback','_emscripten_glResumeTransformFeedback','__ZN10__cxxabiv112_GLOBAL__N_110construct_Ev',0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0];
var debug_table_vf = [0,'_emscripten_glClearDepthf$legalf32','_emscripten_glLineWidth$legalf32',0];
var debug_table_vff = [0,'_emscripten_glDepthRangef$legalf32','_emscripten_glPolygonOffset$legalf32',0];
var debug_table_vffff = [0,'_emscripten_glBlendColor$legalf32','_emscripten_glClearColor$legalf32',0];
var debug_table_vfi = [0,'_emscripten_glSampleCoverage$legalf32'];
var debug_table_vi = [0,'_DisposeSentinelInternal_Finalize_mD2E65502B56A0FF277BC789F66CBAB7EF35EB921','_TinyEnvironment_OnCreateForCompiler_mB0880714FC21EF8066C0BBF2F51A5CF0382AE3C4','_TinyEnvironment_OnCreate_mE5BF46A04BD56CD3D04C6D4418F899B976619C6A','_ComponentSystemBase_OnStartRunning_m444D54487CDBE4E69F22B7CE24D26B2ACFEAAD91','_ComponentSystemBase_OnStopRunning_mADC428F879E52AB8D0103F647D8057728BE1A6C8','_ComponentSystemBase_OnStopRunningInternal_m6C0C5C4EACE1CEBDF4A82B73C527BC11CCB754C8','_TinyEnvironment_OnDestroy_m405939C725A5165AEF263BDA09427E050944C0ED','_ComponentSystem_InternalUpdate_mFF4B19598EF6229D3ACB060B9285E35F607F927B','_ComponentSystem_OnBeforeDestroyInternal_m61F5D829C76EB3A9967E8EBBAC730D8BA19BC879','_TinyEnvironment_OnUpdate_mA3C8B369F9DE1DEE88E7337E3A26837C7AECD6C7','_ComponentSystem_OnCreateForCompiler_m5A314CC02D8829C5426BA9D4A671EB3661231C15','_ComponentSystemBase_OnCreate_m7813FB95A084E66430CCB665649B1AD3B7CF58BA','_ComponentSystemBase_OnDestroy_m1038AF8F050BC12F1996047E1198DD4AB78B38DE','_ComponentSystemBase_OnCreateForCompiler_mFE134D50E4009CC3310CE81556FE55A851D645BF','_ComponentSystemBase_OnBeforeDestroyInternal_m814B47C16DB353F993563CAE40C7AB9A67D48FC5','_EntityQuery_Dispose_m6BD035C2AFE55B94EB5B8CB5257452AB04D79229','_NativeArray_1_Dispose_m64E35E2538530D994E54789648F10A8E58DD92AF_AdjustorThunk','_NativeArray_1_Dispose_mFA65F41DF1E79C480503042F2290B7A924F1CCD8_AdjustorThunk','_World_Dispose_m82C5896980F6CFE6827FB93E354BA327FBAAA7A3','_MemoryBinaryReader_Dispose_mF0518383D1B2BCE8B84DB15D7D63375572DBBA0D','_AsyncOp_Dispose_mDAD7CF618414C0A5D9D0CF2C50AF3E8FFD46CF8F_AdjustorThunk','_BlobAssetOwner_Retain_m282089A386F41519EED1E8BC9267CBBECC33AED8_AdjustorThunk','_BlobAssetOwner_Release_m99EE8FEE6D574AEBD689E9EA01B9F8004712F125_AdjustorThunk','_BeginInitializationEntityCommandBufferSystem_OnCreateForCompiler_m1C73BACF4C7ED8788BC27CE3253D07FD2AED51B3','_EntityCommandBufferSystem_OnCreate_m604AC3ABCCA837D8B9D5C9C8E79DCE187B0D0212','_EntityCommandBufferSystem_OnDestroy_m96E0C32249539B25D3F811F134E1B2E26A7705E7','_EntityCommandBufferSystem_OnUpdate_m89BD414F2D03DA14159D3776A557A8DFDA5DB710','_EntityCommandBufferSystem_OnCreateForCompiler_m1B780F3D2501091529A119366037D74468FF1D34','_EndInitializationEntityCommandBufferSystem_OnCreateForCompiler_m3AF702E887611DFF3A8DB49323A5A122A1452D61','_InitializationSystemGroup_OnCreateForCompiler_mD0F59D1BED38E26AD193B17BFCD26E902141DC08','_ComponentSystemGroup_OnStopRunning_m17EB389CEF9DE3D0D33572C37BF48F6A903A9927','_ComponentSystemGroup_OnStopRunningInternal_mEC5125FE8D9E67BEA042079DB37CFC1BD4BB2973','_ComponentSystemGroup_OnUpdate_mCD92A70C8D7A7DAA659AAFACB3D502643552ABBB','_InitializationSystemGroup_SortSystemUpdateList_m93DC1AAF54898E8495BB9313EEBD7900093717C4','_ComponentSystemGroup_OnCreateForCompiler_mD8C9A497095111A28D96B00656A41E08DAB86D19','_ComponentSystemGroup_SortSystemUpdateList_m0C5C17341A8BFE4BDB5BFBF6C6DA0607326AA3DA','_BeginSimulationEntityCommandBufferSystem_OnCreateForCompiler_mEEF11C9E9D358FD21B962006B643890CE5C7A0A6','_EndSimulationEntityCommandBufferSystem_OnCreateForCompiler_m7DEE35179EEF666CA899FB477830835305597631','_LateSimulationSystemGroup_OnCreateForCompiler_m000C24DEB9786A53CEAC9ADE80EA4A7851317F26','_SimulationSystemGroup_OnCreateForCompiler_m7749044310B1019E95DFE5B429CFD680A282EB2D','_SimulationSystemGroup_SortSystemUpdateList_m4E9A0BA78978F513B9097AF6A112B4C65EFBEBD1','_BeginPresentationEntityCommandBufferSystem_OnCreateForCompiler_m331C1B6A9E90D78D696948368D3E81B5F6EE3C78','_PresentationSystemGroup_OnCreateForCompiler_m4852FB43EE3BD1E707316D5540053D2907633EC4','_PresentationSystemGroup_SortSystemUpdateList_m103F36D8CD7105291987F9C8549378A4115FA793','_RetainBlobAssetSystem_OnCreateForCompiler_m00DCCB8EDE56F0EBCD65E506D33C5A09931F8FA2','_JobComponentSystem_InternalUpdate_mC7BA491FBF71BF25B78043A11DB1E3D07855F6BD','_JobComponentSystem_OnBeforeDestroyInternal_m5E01F27CF427A54EC925A0C08BA687A4CE1C62F7','_JobComponentSystem_OnCreateForCompiler_mC3E36DD6BE3207B8B23A153B2E2C824827A7B844','_EntityPatcherBlobAssetSystem_OnCreateForCompiler_mFC1FE67CE27BA68189A300B073A6E0FC591DBAAC','_EntityPatcherBlobAssetSystem_OnCreate_m94D83DDA7311F0E5DCF7DEE277A9E1F393F47946','_EntityPatcherBlobAssetSystem_OnDestroy_m82CD8B9B0482F25BBA5BC3658FF08738141FA9B6','_EntityPatcherBlobAssetSystem_OnUpdate_m62EA61D5EF6F2DEA0D2D8865AF43BBA4F1E9D4B0','_TransformSystemGroup_OnCreateForCompiler_m29557429F0A6FFA9BFB10809187B596106114BC1','_EndFrameParentSystem_OnCreateForCompiler_mE46381FC1A2D7C06265F325181BD0B46517CAA37','_ParentSystem_OnCreate_m3BE707375EF12FAC339C65B204AC10584B896E9D','_ParentSystem_OnCreateForCompiler_m6B27CDE536BA9254D98C9A84898AF9FBE4389664','_EndFrameCompositeScaleSystem_OnCreateForCompiler_m92B1DE739E3867049CD37581BC919F92BD7A0C9B','_CompositeScaleSystem_OnCreate_m7E3D9629E258282EB9913E894901DCC8D4C74315','_CompositeScaleSystem_OnCreateForCompiler_mC357402DC518B4884299F7F52A1794BB3B961DE2','_EndFrameRotationEulerSystem_OnCreateForCompiler_mA2280AAE76320C754DD85ABE5CBC7C4391214A3F','_RotationEulerSystem_OnCreate_mC28EEA5E03F35A7FF59825DC14FE055BB91FF62D','_RotationEulerSystem_OnCreateForCompiler_mFF925B204F0F02ED022735319797A60AE0769BFB','_EndFramePostRotationEulerSystem_OnCreateForCompiler_mB777A725C428667D3DC5599BF9BAEB4B3A08F1EE','_PostRotationEulerSystem_OnCreate_m939944EDAB14F3CEFD4024218836E256C12ED515','_PostRotationEulerSystem_OnCreateForCompiler_mEC160730249F3A5B722721A846E864F8E5C67D16','_EndFrameCompositeRotationSystem_OnCreateForCompiler_m9CA0EEF6E09767CBA72BDB428E2D470E106BE83D','_CompositeRotationSystem_OnCreate_m95348C2D99A201D56EF4D4C4DCD714E865304968','_CompositeRotationSystem_OnCreateForCompiler_m8E692D049992317CCD9AD6AD96A2BDF035D15A46','_EndFrameTRSToLocalToWorldSystem_OnCreateForCompiler_m58D71199AF5F173E6824BCDFE5DDC5F24A3F2084','_TRSToLocalToWorldSystem_OnCreate_m9FD8088A1B4AC080E22127C0FD086986556990EB','_TRSToLocalToWorldSystem_OnCreateForCompiler_m4BC26FEFB874F2FE88CD739C82065C5E0C126A21','_EndFrameParentScaleInverseSystem_OnCreateForCompiler_m1C019F0322FFB68A1611BA0DD4CC9BD75C3C594F','_ParentScaleInverseSystem_OnCreate_m930F7E0240FE28D5B857CAF4B28EFD3EB0545FEB','_ParentScaleInverseSystem_OnCreateForCompiler_mAE43D6CBA1016FF3B772A990DBAC2568E9DC72F2','_EndFrameTRSToLocalToParentSystem_OnCreateForCompiler_m8FCD2F10552A10F7942F8E8B38990C629B23AA62','_TRSToLocalToParentSystem_OnCreate_mC0848A3F7503A473F38A5BA9DE0567B7F44C161A','_TRSToLocalToParentSystem_OnCreateForCompiler_m13DC2FDC530F6FBB92509EA5AD431C0FFECCB171','_EndFrameLocalToParentSystem_OnCreateForCompiler_m8593D34F8116D93AE6301465498BABA43FFA1CF9','_LocalToParentSystem_OnCreate_mFD39D74434578C6167F9DAB043245ED9EF49775B','_LocalToParentSystem_OnCreateForCompiler_m7D70EDB955F64BDD28FDA2FF09E52B0AC9372D3E','_EndFrameWorldToLocalSystem_OnCreateForCompiler_m1FDC5E7441BC5BF20BD253A168AD90CA07CF1953','_WorldToLocalSystem_OnCreate_m794B81502374106360FBB863B19E429BD207898F','_WorldToLocalSystem_OnCreateForCompiler_m1948C95C7B6A6F5FE6204F4B5B4AADDBD974F51A','_InputSystem_OnCreateForCompiler_m7FB224C10931E4441A33095F1A12A88C176A642C','_InputSystem_OnCreate_mFFFD2B37DB944CCED5C878937DA9E71C8C252129','_InputSystem_OnDestroy_m7386E4E1235B75EED5CE117CF1C396C1606C8843','_InputSystem_OnUpdate_m1EA55A7BCFBC8736733D4BB1359F2B0395A6AFF7','_HTMLWindowSystem_OnCreateForCompiler_m73995D0248B4A7CE17341CA8F13BEA3566797BAE','_HTMLWindowSystem_OnStartRunning_mD8547572760DBCAFD77460CA03E604A352CFE2C1','_HTMLWindowSystem_OnDestroy_mFA1493ED1C96C079D3F884223878CCB117A7C9DB','_HTMLWindowSystem_OnUpdate_m31AFF29FE45D0AB220A04E967B8D08FCBEC01522','_WindowSystem_OnCreateForCompiler_m1619FBDCA276B075946BB73FAFD88A3685AF005E','_UpdateWorldBoundsSystem_OnCreateForCompiler_m49827098F480BC59CB99AEB37130E7C8B5A797B6','_UpdateWorldBoundsSystem_OnUpdate_m54A435015F57E77BF25A9F4E1E5C92D1F92F7AC8','_UpdateCameraMatricesSystem_OnCreateForCompiler_m9E1E1051CC9D2E8E6A00F08AD5C730CE946B6896','_UpdateCameraMatricesSystem_OnUpdate_m0DFAB3819D0EB7291DF84F4F681B578507DBBCA5','_UpdateAutoMovingLightSystem_OnCreateForCompiler_m7E139E2CD50F8BD01B08201F82084E618404507E','_UpdateAutoMovingLightSystem_OnUpdate_mA11128052BD5D44579ED73088A2AB72EA0906ED4','_UpdateLightMatricesSystem_OnCreateForCompiler_m4B55E5B0325A04874B92B33F97AF171DE3CB190C','_UpdateLightMatricesSystem_OnUpdate_m23CEB57165CE6E714C67F9424A554EB3B253AB09','_AudioIOHTMLSystem_OnCreateForCompiler_m78CCE2DD5E38844DFBE147365634639D17CB1553','_AudioIOHTMLSystem_OnCreate_m2EF73001463DE6B93BD34E71847C4C18B9926850','_GenericAssetLoader_4_OnUpdate_mCA7AA3629ABBC1FA2248E93CB8B99FC0415BB694','_GenericAssetLoader_4_OnCreateForCompiler_mEA56BFB7F842939A593E86597E61E16974C02F84','_AudioHTMLSystem_OnCreateForCompiler_m35F2EBE6196CE95A28326DE2F5C0578355141097','_AudioSystem_OnCreate_mF662A93EBC44E17A8E859F701F2E8BCC63840402','_AudioSystem_OnDestroy_mBE3AE4D739D74D57A3FDFCE228F634FB6B40478E','_AudioHTMLSystem_OnUpdate_mE810620A1A88CA0B36C6E8474EF0FA36F668EF0A','_AudioHTMLSystem_InitAudioSystem_mDA131B1760C61D1C24DEBD058E77C66F656EF332','_AudioSystem_OnCreateForCompiler_m5FFCC8005289075FB20059DE691029780F6BDDE5','_AudioSystem_OnUpdate_m25599F14FA337046B519CDE06C7CE1ECB248A90B','_HTMLInputSystem_OnCreateForCompiler_mAFF73349979CD00145A2764CA046C1B007312D20','_HTMLInputSystem_OnStartRunning_m7477F58E4AF1F8B65CE5780810B3E19897874CA8','_HTMLInputSystem_OnDestroy_m01557B3483CB81F07C640FD3C9D0470AE98B5273','_HTMLInputSystem_OnUpdate_m39D6CA32D6CF6D0F159B00A9AB3B499BAAF4C15D','_EntityReferenceRemapSystem_OnCreateForCompiler_mAC437DEAD10D594FE596386DE90128E5CFE2EDFC','_EntityReferenceRemapSystem_OnCreate_m5F0440027313A18C0F89B9CE4EF894B817C55E08','_EntityReferenceRemapSystem_OnUpdate_m7FFD7B2B38D7FD68BA290391E457FC20036D2215','_ClearRemappedEntityReferenceSystem_OnCreateForCompiler_mDD3629B66C35CB811374E609C7A3CCBC85592551','_ClearRemappedEntityReferenceSystem_OnCreate_m5199BBD0F9D4E679F54543B5CCE66087F001D8D9','_ClearRemappedEntityReferenceSystem_OnUpdate_mAE9CB30C9018B26CE5A53493F988D4F4BF579AF2','_RemoveRemapInformationSystem_OnCreateForCompiler_mEC548C20BE96DFBA480C1E6F5A46A3F5B1D3B720','_RemoveRemapInformationSystem_OnCreate_mBAC71C486C2DBE02EA95D7456CE196CAB10E8241','_RemoveRemapInformationSystem_OnUpdate_mBB109BD2472C77492FFEC47F26E82EC6162A158B','_SceneStreamingSystem_OnCreateForCompiler_mBCB6054440E873A7D783A92023A2C107DF59E63C','_SceneStreamingSystem_OnCreate_m95AC3FF01EE9A45AE00A5B3F9904FF1BD3B68B61','_SceneStreamingSystem_OnDestroy_mBBB58365545A694578F323FE26DA7D75F3FB6306','_SceneStreamingSystem_OnUpdate_mCF55A79992062267AE85863BC662FE59298D6E65','_Image2DIOHTMLSystem_OnCreateForCompiler_m068DA05E97351A1EAEC6C7314D6AE6711DF1EE11','_Image2DIOHTMLSystem_OnCreate_mC1037C08D62E0FE8EFB6BCA5D4C96E976FCA591C','_Image2DIOHTMLSystem_OnUpdate_m6FC2205C1B31312861C8A0655D3774343BFDFC60','_GenericAssetLoader_4_OnCreateForCompiler_m171FCEAD177FC268772D0E06D7207D84F7DCA61D','_GenericAssetLoader_4_OnUpdate_m23D3C8E76EAF999C84A7FDAE96F23CFB4D7207A9','_UpdateMaterialsSystem_OnCreateForCompiler_m7891C3A9DE429FA7B2176C003B7B774F94F0D9BB','_UpdateMaterialsSystem_OnUpdate_m5CA74B197052D9718308930BC99D7ADE51A5206F','_RendererBGFXSystem_OnCreateForCompiler_m0DBB6313CCEA18E81D02F73AFB18F7F16E8A8391','_RendererBGFXSystem_OnCreate_mBE8D17CCD840748674D35D4D94B0BBDD3C5CCCAB','_RendererBGFXSystem_OnStartRunning_m66D0AADA13510BF209FAE3E5ACEE7CE856E05493','_RendererBGFXSystem_OnDestroy_mBC00C011EFBFA21384B321024F03681B90E7B3EE','_RendererBGFXSystem_OnUpdate_mAA1D59A2CB3FDB5936F22F4DDAE3BE8B56E143F1','_RendererBGFXSystem_Init_m67444CFF169CA91D49A2E80C4CA686F53AA53FC7','_RendererBGFXSystem_Shutdown_m02448BFF3638E294AE74D977225450125314A8B4','_RendererBGFXSystem_ReloadAllImages_m85629DDE15A222FCDDD25C221DCE6056B5EFBA0D','_RenderingGPUSystem_OnCreateForCompiler_mE1E3AB5E17735FA76E0A8E2DF1E63A2E9E6B0BE8','_SubmitFrameSystem_OnCreateForCompiler_mD195084225C54E763C47123354F17D4544002E5E','_SubmitFrameSystem_OnUpdate_m57131C67A0F7BBB8F762116BF9986C2BDBCF14F5','_PreparePassesSystem_OnCreateForCompiler_m07CFD9E2561A1BA8E185B63823BDD65F45751217','_PreparePassesSystem_OnUpdate_m57165E3F06F223EC545006E1D17865AB5708D554','_RenderGraphBuilder_OnCreateForCompiler_m48CBEC44C5439B18ADAA68E22628347641F27A49','_RenderGraphBuilder_OnUpdate_m2807BE5F7031EDD1A6144FA952A2574F9938806A','_AssignRenderGroups_OnCreateForCompiler_mA9EC16D1680F11EA741C0473FA66AC37F9648467','_AssignRenderGroups_OnUpdate_mA21DC97921D8CA982180953FBBC6C22716EA3A40','_SubmitSystemGroup_OnCreateForCompiler_m7147D6E78B66A05CD951BDD2351E0D0420CE22C2','_SubmitBlitters_OnCreateForCompiler_mC373AEF05182714AD229DEA10F96F27F031ADDC2','_SubmitBlitters_OnUpdate_m9D7061543CC7F047B7F45740A6FA7A119C1643C7','_SubmitSimpleMesh_OnCreateForCompiler_mA74821BA6D023396209D002E1BB5828B189C7A2F','_SubmitSimpleMesh_OnUpdate_m9D222064A6FDCBBACD782D4F4B02C97C89D288AA','_SubmitSimpleLitMeshChunked_OnCreateForCompiler_m6B42180BEECC05AF51C24FC6B98220F9109B174B','_SubmitSimpleLitMeshChunked_OnCreate_m506A1BD9D2CC4FBB1640C9DE7B1E418D8C176A02','_SubmitSimpleLitMeshChunked_OnDestroy_m486F9798C5430CB5335717832467FE873E26A6BC','_UpdateBGFXLightSetups_OnCreateForCompiler_m98394EA670ABE8AFC871069624B14BE00A0B8B4C','_UpdateBGFXLightSetups_OnUpdate_mFB7877A72DAA2564CB11AE6709DFB96B2AC8E393','_SubmitGizmos_OnCreateForCompiler_mD2DE9CF225473CB0A52638661143A36448BB2A83','_SubmitGizmos_OnUpdate_m31C188A24E23288E9249125F9E33335129E24DDA','_DemoSpinnerSystem_OnCreateForCompiler_mB9D12E94773C0881A46D2742D85B389B5B610A98','_DemoSpinnerSystem_OnUpdate_mCA820ECCBF5EB240726E9FE7DAEAC94E61BBA822','_KeyControlsSystem_OnCreateForCompiler_mF4D73E67AC8AA41553DFF7C13DB7A2DADD0CCC21','_KeyControlsSystem_OnUpdate_m5279C7FC1210587522A64801ACC4E2E2C014C3FF','_BumpCar_OnCreateForCompiler_m1179FBF21D533D2E04CAC96FB64E63393D585D3F','_BumpCar_OnCreate_m796DE0EF43838ED4469855DBAB7E16F4E9C65E98','_BumpCar_OnStartRunning_mFECAFAA883903D492CB3F83CD25B46BBDED45176','_CarAudioSystem_OnCreateForCompiler_mB6D12AACE1C3AEF024183943E0BAB4241D345A76','_CarAudioSystem_OnUpdate_m802411CBFDEEF9D7DD53CE5EC940BE9E2C2ACFA6','_CollideWalls_OnCreateForCompiler_m34F217D945BAE09E3A83BEB7B3917E0D860B8578','_CollideWalls_OnCreate_m5F7367E4D9840B81E1217693A6F0C301725F4DAE','_EnterBoostPad_OnCreateForCompiler_m4A22477BF2ABAFDD354EF955931AD65CC1307FDA','_EnterBoostPad_OnUpdate_mEB8651A6F9933AE1C7EC824A31E72E347B5AD6BF','_InitializeCarResetState_OnCreateForCompiler_m88C4E39E4D260A2C3E9A255735A6268ADB105527','_MoveCar_OnCreateForCompiler_mFDD243B29310A8D00E46D783C9CEFB37930D6EA4','_ResetRace_OnCreateForCompiler_m25CEBC00C9A55763F8881963127BC03578D0C420','_ResetRace_OnCreate_m2F6DF4B80110CC6B387AC426F4D8998FCA73A4EE','_ResetRace_OnUpdate_mC1153B21E58F84AB5D62758A6E786A24DE8C847D','_Rotate_OnCreateForCompiler_mC88DE1D36A8D6EEF3109F310BC0A5B7FE64E57B4','_RotateCar_OnCreateForCompiler_mC54B6A00BDAAA707EFD4FCB40D80D0788D1919FD','_RotateCar_OnCreate_m3027D48770A01EF51A57C8176901EA67EE7FA851','_SpawnSmoke_OnCreateForCompiler_m6C6A34598EBC53EDE719D2D154F11373E8BCC9CA','_SpawnSmoke_OnCreate_m4F1B97051DD2FF4D9C6CFD02AC8506B783A6A835','_SpawnSmoke_OnUpdate_mB04C8CCF179ED76E6DDB7DC650BC1803C3E79CB7','_UpdateCameraFollow_OnCreateForCompiler_m6806CA4C68DBCA8EDA4D51A25F8C176181B1ED36','_UpdateCameraFollow_OnCreate_m28A5017BB2E75973AE2A33167AD5ABD15459A70E','_UpdateCameraFollow_OnStartRunning_mE1C663AE9812811975F6354B1ADB361384FE7ED1','_UpdateCameraFollow_OnUpdate_m33AB4E83AA55EF93063807956638DAFCDA4D948C','_UpdateCarAIInputs_OnCreateForCompiler_mA3A9DD4BBFDF852399F05381494AC664F0CC0429','_UpdateCarInputs_OnCreateForCompiler_m1F72DE604C48A98349D7B61B501C1C82149BBDBA','_UpdateCarInputs_OnUpdate_mF2856BF0F8727B6CB31358D13F4AEC4E14F2ACD1','_UpdateCarLapProgress_OnCreateForCompiler_m62C376393555A8A5A2425EE94DF42E7202C77B8F','_UpdateCarLapProgress_OnCreate_m4A86440DD00D89506284314943281E100B5DBF70','_UpdateCarLapProgress_OnDestroy_m878497AFAE0C3A370326EC2CF5B695481E06B0CA','_UpdateCarRank_OnCreateForCompiler_mC7A9E07F041C10120D5C5935F61CBBEB47C025BF','_UpdateCarRank_OnCreate_m0D863FA2D97BDD03E097C011700474ABC86FAA73','_UpdateCarRank_OnUpdate_m6DBEEA3CFE3B9FDA1515C1D29E0772E34680BECB','_UpdateGameOverMenu_OnCreateForCompiler_m3CDCA938B5CD0D3F369C4804F502BEC77CEFD2F7','_UpdateGameOverMenu_OnCreate_mFBF17FE18B5A90A1EEBC41F9692E73C35EC3C61F','_UpdateGameOverMenu_OnUpdate_mBFDFAE0F29EDEEB048F2969105520C0A63FC1F8D','_UpdateGameplayMenu_OnCreateForCompiler_m38AC77CC991C9E9F14E61E3832E1CF7C4BA006AF','_UpdateGameplayMenu_OnCreate_m2D77C21CA4B03E594900C40A2A01262D67F68DD1','_UpdateGameplayMenu_OnUpdate_m7B0E90448DBEC99852A40566CD0E55F8DAC8C28F','_UpdateMainMenu_OnCreateForCompiler_mEA3DFF206F0000BE49E11C3F795E5A7BDAAC050A','_UpdateMainMenu_OnCreate_mEF0C7EF100E7E9EEB1D32B7A37A4987D78BF88FC','_UpdateMainMenu_OnUpdate_m424436F9F74C1C86FF5C4528E62DB2A720BC2AF6','_UpdateRaceTimer_OnCreateForCompiler_m24375B8DBC78318B53864B8680B5D7BBD7564CCF','_UpdateSmoke_OnCreateForCompiler_mABEDB3A07B6D51C83427552BBDBB82F991CD0BC9','_UpdateUI_OnCreateForCompiler_m53436123873761CDD49327550AC3F99D554850D6','_UpdateUI_OnCreate_m811C935CC324D54466E9197E913115FCFCB35148','_UpdateUI_OnUpdate_m741D812AAFB4289FBD63E835B33EF7E199D9FA34','_InputData_Dispose_m8113B6FA683656AEB6E21E7329E016C25C985B76','_EntityQueryManager_Dispose_mF1D0A82EED06A0E24829D25D2C6CE6F5FEAF3AC0','_NativeArray_1_Dispose_m2C63F421803097D24401C1B67CAC322D8E7F3831_AdjustorThunk','_NativeArray_1_Dispose_m9A8A96A09418C9DE6ED4618767BEC03C1580747C_AdjustorThunk','_InsideForEach_Dispose_m04D005E8B2FE6DB8BA7154ADC4B8DF759694EEBC_AdjustorThunk','_NativeList_1_Dispose_m5CC6C36BC8C118E980E1A9FA711C599E5E098438_AdjustorThunk','_NativeArray_1_Dispose_mA416CC5816E45BB4080341CD481888CF4899917F_AdjustorThunk','_EntityCommandBuffer_Dispose_m5BA38D9DF18BE55B4BD004DC6BF17DE4F303312E_AdjustorThunk','_Enumerator_Dispose_mF8E60D3D0C5890B085C086D26251E623E15A686D_AdjustorThunk','_Enumerator_Dispose_mE2292A2CE595BB532E64DB61E0087A376F8A59B0_AdjustorThunk','_Enumerator_Dispose_m11AEA0EA9CD7510857F08110C7EAF60DA4411A8D_AdjustorThunk','_Enumerator_Dispose_mD546676A7AB61FA26E8D8B1EC0FEAF6B28E6249C_AdjustorThunk','_Enumerator_Dispose_m1149CAC7CA990C397783103210BA20536B9D4577_AdjustorThunk','_Enumerator_Dispose_mB6A5BE4768C9C19AE6D039001141D8DD82E65B97_AdjustorThunk','_Enumerator_Dispose_m6F426FBE30647A697F041056380521058E469B8F_AdjustorThunk','_Enumerator_Dispose_m2C2C02CBAADD5B9DEA07E38A0B5A333B0FC534A9_AdjustorThunk','_NativeArray_1_Dispose_m9D8B8856DBDD9D5BE2C9F67AFBAEB9332449DF02_AdjustorThunk','_Enumerator_Dispose_m3ABA2D1CF3BDC8AF769795D93EEDF088CF9458B6_AdjustorThunk','_NativeArray_1_Dispose_m460A5A8DCC4C78F64C6D59748C648548F55BF4EE_AdjustorThunk','_Enumerator_Dispose_m5530E7D420383B04D093CBC2AE8018C40CD6DF83_AdjustorThunk','_Enumerator_Dispose_m739E8861730CEECE453DDFF1D88D1C33DDB77A21_AdjustorThunk','_NativeArray_1_Dispose_m728F23AB2FE13474D35BDD2EB5AF20C6715144A3_AdjustorThunk','_Enumerator_Dispose_m738BD1C9918C2C70FB994DF5821F68A06F07EF66_AdjustorThunk','_NativeArray_1_Dispose_mCEF67491284356F2B54B3E33A10EF050CF20FBCF_AdjustorThunk','_Enumerator_Dispose_m6A30012C5E596447FA5AD53638E806E328CC271B_AdjustorThunk','_NativeArray_1_Dispose_mDFDD8CF7FA42B6145E73E91EB9D8130268CA1388_AdjustorThunk','_Enumerator_Dispose_m101911CACACFEB2E98D0EFAE09A089CC7211A04A_AdjustorThunk','_NativeArray_1_Dispose_m33533FBCDA2C32DEBBD146F236CA7AE901D13B52_AdjustorThunk','_Enumerator_Dispose_m3D443779F98EBA04047002F712BFE97A21A17C25_AdjustorThunk','_NativeArray_1_Dispose_m7C42E16828C6ADF7C7CDCC5077875F5EBCC62DD7_AdjustorThunk','_Enumerator_Dispose_m55370E8EDDB1760C357D8309F3A3E4DC6AF9CFC9_AdjustorThunk','_NativeArray_1_Dispose_m7537CFD172C9CEB31C2535B9A3584131179EBDCE_AdjustorThunk','_Enumerator_Dispose_m207BB54B9979BC60E5544B5A5648E54F23168341_AdjustorThunk','_NativeArray_1_Dispose_m29E4AF43CF2C1DB4C1AC282D7F5B0D86AE44177E_AdjustorThunk','_Enumerator_Dispose_mDC1970B4A506F42B1551F3B924A48962A555EDDE_AdjustorThunk','_NativeArray_1_Dispose_m88572F9E8BD65F58020B2ECF227EC45CC7B3F96B_AdjustorThunk','_Enumerator_Dispose_mE268B75A3C36500990E9B2C5ACA571876617C109_AdjustorThunk','_NativeArray_1_Dispose_mB8671854186C7615EF8E493C3C2323382B73BCEB_AdjustorThunk','_Enumerator_Dispose_m52807A471176CA3F706203E02A79D1F8DE9B9569_AdjustorThunk','_NativeArray_1_Dispose_m42E732373098F73C5EE4C0F71E1C1826495778F6_AdjustorThunk','_Enumerator_Dispose_m8DB2EEBF15D1B53FCA493C4E19DEAE723D450C2F_AdjustorThunk','_NativeArray_1_Dispose_m087A51987A18C50835DE924F9C2618BC56EB62A3_AdjustorThunk','_Enumerator_Dispose_m17F22D470CD4AAE1D6BD41FFF1DA49D33792FD73_AdjustorThunk','_NativeArray_1_Dispose_mB2BEA33140079D03E7BE82CACC82685D4C57F103_AdjustorThunk','_Enumerator_Dispose_mAB60354023F8E32292A5CB950834CABDDA2C0C77_AdjustorThunk','_NativeArray_1_Dispose_m90826A151AE8E3BD2883CB04A418A29DD8DF537B_AdjustorThunk','_Enumerator_Dispose_mD5D7742018E45C42315063E041010B9782D1EB89_AdjustorThunk','_NativeArray_1_Dispose_m3CAED43513958DBB4BF3DCA3036FAC46E7DDED3E_AdjustorThunk','_Enumerator_Dispose_mC8A0B38357C3CE2810B9A18DFAE2786AF4F22167_AdjustorThunk','_Enumerator_Dispose_mEA347921B9678F1A4CEA7234EC4A641AC8C17115_AdjustorThunk','_NativeArray_1_Dispose_m0C3473A018E8E908D3BCDD450272D1E62326CC28_AdjustorThunk','_Enumerator_Dispose_mD288CFDE1E1DD4BBFF26DAFF41B2AA3DE05E31CE_AdjustorThunk','_NativeArray_1_Dispose_mFAE53D9FA271E2E5D8166D7DF5FEC37AB5DA185B_AdjustorThunk','_Enumerator_Dispose_m13E8903E597F650C1AF21461BD9B96D0D83BF6D5_AdjustorThunk','_Enumerator_Dispose_mF59B00628A0231BAF7986BC3FED771078165AE7A_AdjustorThunk','_Enumerator_Dispose_m9FD72A42832C3FBABEEE4A7ED6B2176E3D081DB3_AdjustorThunk','_NativeArray_1_Dispose_m648401B552DEA4D8431A595C9359793D03C302F2_AdjustorThunk','_Enumerator_Dispose_mC1DA238F5983A6A6CFA4CC604FC95E2EA3F7F0B1_AdjustorThunk','_NativeArray_1_Dispose_m34457986ABFB911A25E3DE286CEBDC56F5796B6B_AdjustorThunk','_Enumerator_Dispose_mDFB443D1455D447648437DE3D228AB254FE0E9A0_AdjustorThunk','_NativeArray_1_Dispose_mBF7533369EC7FD2BF5C194BAB9A70030053E6F33_AdjustorThunk','_Enumerator_Dispose_m509BE0D38CF632FC080EC33267A6DC6F44E41EE6_AdjustorThunk','_NativeArray_1_Dispose_m2195E339A3FB67D50750A6A756B720DCF13F31DF_AdjustorThunk','_Enumerator_Dispose_mBAA165B06CFF663358E523EE1061E2AA039E4CDA_AdjustorThunk','_NativeArray_1_Dispose_mF916C6EFF1F2BAA826A453E388B6BA7D2CA6AE1A_AdjustorThunk','_Enumerator_Dispose_mD7F7970CB75BEFD72938C9A8FA48E8CC9B0D8434_AdjustorThunk','_Enumerator_Dispose_m3EC1D5C9F73912AAE212354B9E90F811FB1D3C83_AdjustorThunk','_NativeArray_1_Dispose_mD1A12E30F0BFE17DA7F753A7AA1916BBA554FACD_AdjustorThunk','_Enumerator_Dispose_mF29B537E8F97986ADF39F24A248D983B998B606B_AdjustorThunk','_NativeArray_1_Dispose_mF21451077958AA08C2E886A28EF42D3285301DE4_AdjustorThunk','_Enumerator_Dispose_m196B12AD89E684C460A057D0266F8D7D586B334E_AdjustorThunk','_NativeArray_1_Dispose_mD7C1CFCD6A9EFB2483813FD4990F45413C91E46D_AdjustorThunk','_Enumerator_Dispose_m2EF99CB7B00D3877F9222CDCEACB9C789A35EC22_AdjustorThunk','_NativeArray_1_Dispose_m890318A0A778C22300A643458F4A791E284F87B3_AdjustorThunk','_Enumerator_Dispose_mC8D040F320C4A6A741713B8D20C6F8E17D1F2883_AdjustorThunk','_NativeArray_1_Dispose_m5DA362D3EB78A34E7C43B45FD6A59D2CCD8F1BDC_AdjustorThunk','_Enumerator_Dispose_m1C6B687063619DF8C062DE76CD899430EDF5DFB8_AdjustorThunk','_NativeArray_1_Dispose_mE2EBCC75FEC213420AB1CC5E887923B862B86FCA_AdjustorThunk','_Enumerator_Dispose_mA7A8B9C98F173C805F745B6FE85988D5F9D3EBE6_AdjustorThunk','_NativeArray_1_Dispose_mEE9115483F79F9BB2E1D8628016029BEC42D6384_AdjustorThunk','_Enumerator_Dispose_m401387BF3F1AA4CEDA632FE907579BE467C1E5A5_AdjustorThunk','_NativeArray_1_Dispose_m7DC31A3BAC8686B1CE634FA024A6809E97460C6C_AdjustorThunk','_Enumerator_Dispose_mE8AC07BFFBB32AE63DC91E3F45FD217B06494E12_AdjustorThunk','_NativeArray_1_Dispose_m155005186EC2C7880359E448F24218611EEDF994_AdjustorThunk','_Enumerator_Dispose_m597D766BCC0A98929D312F3E6B07D52B1E6D5C8E_AdjustorThunk','_NativeArray_1_Dispose_m19F56504F81D6431EAF0A2D6C057C61C5B2D8FA5_AdjustorThunk','_Enumerator_Dispose_m7765481B9D61D103F19D6B442E15A1ADF833A131_AdjustorThunk','_NativeArray_1_Dispose_m6B21AF75FCF0368729787B894A5D88440DC6E150_AdjustorThunk','_Enumerator_Dispose_mD368E96CF96F0AED3EA6497C41214E74BE676C27_AdjustorThunk','_NativeArray_1_Dispose_mB7B71B49472DB799B68A272C17F5DDBDFB0FF5F2_AdjustorThunk','_Enumerator_Dispose_mC8D97EF336FC2806D1254841D0FACB29B8102ED6_AdjustorThunk','_NativeArray_1_Dispose_m666C747AF9D1EC2CCFC8D15012A97340AFDFC3C9_AdjustorThunk','_Enumerator_Dispose_mE686F2ACCEEAC8FF0054A50764DB3DF672A36C2A_AdjustorThunk','_NativeArray_1_Dispose_m9BA025104FF8134CCA0EC29AC76F4AEC156B051F_AdjustorThunk','_Enumerator_Dispose_m0785FE74830ECC629401DE18C1FD1A3C4991C8AC_AdjustorThunk','_NativeArray_1_Dispose_m60A26625937C06EBED751B7A220D5664356AEB01_AdjustorThunk','_Enumerator_Dispose_mA8BD0EDABE64ACE8D8F7F376B674A70146A97D49_AdjustorThunk','_NativeArray_1_Dispose_m6FCFF215C4DF85D07FDBE94A0FEDEEFB4DA1FFAE_AdjustorThunk','_Enumerator_Dispose_m8B3F8E15D032FBDBDDACAD90571728EFF5FB27EE_AdjustorThunk','_NativeArray_1_Dispose_m3B888D120857F7092480363D5045E76BBAA82119_AdjustorThunk','_Enumerator_Dispose_mFC4D547E5149827851DF9B91AAD459323B405C60_AdjustorThunk','_NativeArray_1_Dispose_m1E7FE018B272BA62C2208D56C48F03102B0475D7_AdjustorThunk','_Enumerator_Dispose_mC3E4F8FA82C0CFA1B8018E68393AD7E9FDEE766B_AdjustorThunk','_NativeArray_1_Dispose_mFBC41B9171101D16F5E44A3FAAD4E77C0B15A932_AdjustorThunk','_Enumerator_Dispose_m6B12D432A05664FFDCD595F9B3110322C63F5465_AdjustorThunk','_NativeArray_1_Dispose_mF4F444876183ECE167F6900B17E6C74C8B1FDC57_AdjustorThunk','_Enumerator_Dispose_mAFD1F0595A94DE3B3BBC12FD6AF61700EAD32868_AdjustorThunk','_NativeArray_1_Dispose_m866127201BDA09401D229376477EE9B0DDC3CF59_AdjustorThunk','_Enumerator_Dispose_m47B4510CD7775B85D926573028F3809DDEC2E963_AdjustorThunk','_NativeArray_1_Dispose_m5AB07740E9CE184D7B820C862FFEBB376C76A808_AdjustorThunk','_Enumerator_Dispose_mF8CD3EE275032B2F8CF5F5FC30932F1386C2FDA5_AdjustorThunk','_NativeArray_1_Dispose_m617488B5958413038D64DDE45BC26BE9B383F6AA_AdjustorThunk','_Enumerator_Dispose_m40162DF9BBE1F33D05E3080EB6225889467C21A9_AdjustorThunk','_NativeArray_1_Dispose_m3A32BB2A430D164BEAF57A7C63B771CB04ADD8EF_AdjustorThunk','_Enumerator_Dispose_mA1B1EBB6BAC843EE5E28F8CF498E19D446063E7A_AdjustorThunk','_NativeArray_1_Dispose_m200D4670537CACBAA039C69281AEA7300CC970F3_AdjustorThunk','_Enumerator_Dispose_mDBDBEE62F1E0729176F34E03EEEE52C99C4E09DF_AdjustorThunk','_NativeArray_1_Dispose_mCDFFCB12556F89B6099A792B8F20EA9AB7D7D121_AdjustorThunk','_Enumerator_Dispose_m4108F190BE325B6D61E741CB02834237726663EC_AdjustorThunk','_NativeArray_1_Dispose_m4742EE31D35E13D583C811BC8FE31586F564CB57_AdjustorThunk','_Enumerator_Dispose_m2BAE433CA6D0E98C52594D2690990DF6DAAA7974_AdjustorThunk','_NativeArray_1_Dispose_m9EEB9FB23ABF50A35DF21EE0398F41283EF3BB1F_AdjustorThunk','_Enumerator_Dispose_m7FD12C71F6A5B3447837B7EE423CEFEF75312B51_AdjustorThunk','_NativeArray_1_Dispose_m12A1414FEDC79E95AAF51D2A0B52FD8149B44D45_AdjustorThunk','_Enumerator_Dispose_m1D065193B733672E15BFC25F8F3ADB423847659A_AdjustorThunk','_NativeArray_1_Dispose_mCB487F9A23B8888EAC187699AE4014BA86E859F9_AdjustorThunk','_Enumerator_Dispose_m2CFB55CC60F04750FD071E3A698E0EFC432A583C_AdjustorThunk','_NativeArray_1_Dispose_m0F4F18526FCBFA8F0E1091B307115CBFD1056A00_AdjustorThunk','_Enumerator_Dispose_m46B7DC91761B596584CF067260697CCA776CE297_AdjustorThunk','_NativeArray_1_Dispose_m147CF5900686051163C57BF5B4C32E4317DDCA61_AdjustorThunk','_Enumerator_Dispose_m88C61ACBD08501A592900045ECF3864AB431EA4B_AdjustorThunk','_NativeArray_1_Dispose_m00767BBF1324F6F140F6ABA815EAC5DF32449841_AdjustorThunk','_Enumerator_Dispose_mA019A10B61DB8B01F65ABEE5D8C19BAC76065FA2_AdjustorThunk','_NativeArray_1_Dispose_m9FF83CDEA2BD245DE016DBADEF48931DAB8C3556_AdjustorThunk','_Enumerator_Dispose_mF265875A8CF320439E03C4258DCA1FCA9D8BE02E_AdjustorThunk','_NativeArray_1_Dispose_mF252487DC5D1B5F9AE7F45C8FC87F5793DD79458_AdjustorThunk','_Enumerator_Dispose_m6FE351967DA9699CE390579F25682A54182C17CE_AdjustorThunk','_NativeArray_1_Dispose_m0F605C75B7FEA660FB66D55CD754977C5141BA6B_AdjustorThunk','_Enumerator_Dispose_mC58C610AB40342F8CE39C71591E8B09B1872E710_AdjustorThunk','_NativeArray_1_Dispose_m972C7291C1C46CA9BC77166C542F67A66F04DEE9_AdjustorThunk','_Enumerator_Dispose_mD3FF10B328F2915285ABF43A2FF27ADC64F5EE2F_AdjustorThunk','_NativeArray_1_Dispose_m14D8D5BDD5039F51DA6571D0353E04B04D90049A_AdjustorThunk','_Enumerator_Dispose_m65FF9731A2CE8C8ACBEB8C3FC885259A5FAA6B40_AdjustorThunk','_NativeArray_1_Dispose_m14C21DD385D6967C93F15C0E34BB8D3DDEC01C1C_AdjustorThunk','_Enumerator_Dispose_mE70C09565A29764A24F14BF3D4AD866FC17ED7EC_AdjustorThunk','_NativeArray_1_Dispose_m5FE2034D7E88A6D2265B32567EC941F6E1DA65DE_AdjustorThunk','_Enumerator_Dispose_mBE87EA8CC60D71B30B9874E3E67897F0676585A2_AdjustorThunk','_NativeArray_1_Dispose_mB63015157E7E0D9DFF7387E56CB932E822806BBD_AdjustorThunk','_Enumerator_Dispose_mB87BFE0FB58E88B68014403C3DFECD585E7EE611_AdjustorThunk','_NativeArray_1_Dispose_mD49960A88ACE4837393873B65F70224F6AFE208A_AdjustorThunk','_Enumerator_Dispose_m0AAED1B1E5D1F305485718C7F59FC8BC62D85F71_AdjustorThunk','_NativeArray_1_Dispose_m45CD6482B5FC1681952ECDEC27AB95758A670823_AdjustorThunk','_Enumerator_Dispose_mA713590D51A4333EB996ED5F91EE1BB76A416E7C_AdjustorThunk','_NativeArray_1_Dispose_mECF503F0929538C1663617B35FE8C354D22D44CA_AdjustorThunk','_Enumerator_Dispose_m0326E61E5FDA0E72B6011FC9D7B536027C418407_AdjustorThunk','_NativeArray_1_Dispose_mE8B1F064CE5ACB68370B8781A13615D2D3F43679_AdjustorThunk','_Enumerator_Dispose_m6F9FCC583F56A2CC4A46631EE60F6D8E92E9B750_AdjustorThunk','_NativeArray_1_Dispose_m85EE2233068A41582D7C79538F65C546930081FC_AdjustorThunk','_Enumerator_Dispose_m9CF48041C8EBE010403FDFDD26BBFE0859B91199_AdjustorThunk','_NativeArray_1_Dispose_m5326E9B6BD5E4B29EC5E1CF5E55B86BCDE20844D_AdjustorThunk','_Enumerator_Dispose_m741F8FD74503E31715631D7814A8479B14FE0AFE_AdjustorThunk','_NativeArray_1_Dispose_m0CB06513FD6B4DAF48E5721ED1570ABBA7DB2421_AdjustorThunk','_Enumerator_Dispose_m9F028372CA8B4759CC47B07E4BA87F475F14CF31_AdjustorThunk','_NativeArray_1_Dispose_mB36C256AB61E521609450DD76CB982E8D2ACF8A7_AdjustorThunk','_Enumerator_Dispose_m0A04F99C1ABA1300636EBAAEB16A46BAF3C2100A_AdjustorThunk','_NativeArray_1_Dispose_m87B7D251CF847B9B717915AFA9778A1502349DBB_AdjustorThunk','_Enumerator_Dispose_mD446F33C987D14C550D3B0CCC4F4DF0AD12A7DDC_AdjustorThunk','_NativeArray_1_Dispose_m2251B05AB5228E5CAEA630EC17C50F40D566FECD_AdjustorThunk','_Enumerator_Dispose_m0F6A92F720346EE9CAECC3D9B70481B4C4850413_AdjustorThunk','_NativeArray_1_Dispose_m0FDE2D82A16B6199BCDA060610B5687A43B941EB_AdjustorThunk','_Enumerator_Dispose_mC312023DDD585E0A415B5A963DB8B3CD3F295A87_AdjustorThunk','_NativeArray_1_Dispose_mB7ADEDBF0E392BA9F993C9C454FA052DB16BA996_AdjustorThunk','_Enumerator_Dispose_mEA054E90377423FF24F6D64E353D71132F202AB2_AdjustorThunk','_NativeArray_1_Dispose_m1FA524C4E5F871E6837B3EADA83007E7F4FD7DA7_AdjustorThunk','_Enumerator_Dispose_mC2DE0B4A6F9CF87F6805EE0D1BB49A3828869181_AdjustorThunk','_NativeArray_1_Dispose_m3AD62E5FE28698DA7608B3B3C5FD1BC87C0B2281_AdjustorThunk','_Enumerator_Dispose_mADE2638D51084F2A56723F16BD9E1FF7D7CBACD5_AdjustorThunk','_NativeArray_1_Dispose_m43D82B5E40294DE1249849A1ACD756B6966212DF_AdjustorThunk','_Enumerator_Dispose_m1BFCE56149A95D4D8F46A6C70EC2CEA91FB97D50_AdjustorThunk','_NativeArray_1_Dispose_m47AAACB91B7AF0EADB6028E3DB5C7EF3277A743C_AdjustorThunk','_Enumerator_Dispose_m899B0AD36DD88B8902AD5DE73D5EC7A8A5E8CAA0_AdjustorThunk','_NativeArray_1_Dispose_mC6CED4EB150C0212941C8559250E2F580E9B81B9_AdjustorThunk','_Enumerator_Dispose_m60DD335D21DCFE7DAD2D780D149B42538C2BD5DB_AdjustorThunk','_NativeArray_1_Dispose_mED2EA978276355A0FD146EAFE26985EFD2B6401E_AdjustorThunk','_Enumerator_Dispose_m44585CB81A33B0954B5A3EBB6D93CB9C57C72C36_AdjustorThunk','_NativeArray_1_Dispose_m1496682FBA56EC0ACF924DFBE7B94809FDF52EE5_AdjustorThunk','_Enumerator_Dispose_mF64B29A0DE4FED4E010A3DA4A140FB1D764B5ED2_AdjustorThunk','_NativeArray_1_Dispose_mED1F2F393DE2D63E6D61EA687BE8256E0E94A86E_AdjustorThunk','_Enumerator_Dispose_m9CBF491A92927B86FD6C07AA686DD33054A4A8AA_AdjustorThunk','_NativeArray_1_Dispose_m4CCB67032DAB978F005A369419C7F615F8D4B5EC_AdjustorThunk','_Enumerator_Dispose_mAFA900C07B53E03B5CCE02901A9D6EBD9DF238EE_AdjustorThunk','_NativeArray_1_Dispose_mB1FED55411DC93D6C5E978DB09260C5D887F4447_AdjustorThunk','_Enumerator_Dispose_mF450BCD212DC5B4AB0427A81CC646B8FABBE9FB8_AdjustorThunk','_NativeArray_1_Dispose_mFD108BB8ED91A10AC96ED4A5B35CCC445DA4707C_AdjustorThunk','_Enumerator_Dispose_m3634C72EE4709DD60C8058683786322EC5EAD914_AdjustorThunk','_NativeArray_1_Dispose_m8D9C062D162BA4FF0348792E7879F8D832515354_AdjustorThunk','_Enumerator_Dispose_mDF2480525EEB0D88B7637E92A3B379D3DC3BB4E3_AdjustorThunk','_NativeArray_1_Dispose_m93000A6E629DA8E3A85414C712336F836410164A_AdjustorThunk','_Enumerator_Dispose_mD6268F4344F627EC3C435C351DE0CE5C1A34D46B_AdjustorThunk','_BlobAssetReference_1_Dispose_m14877223DA74C457874E6080BC5610DA7CB3C1D8_AdjustorThunk','_BlobAssetReference_1_Dispose_m23DF57B782244D9C74617C193FB1CF5B49B20FFE_AdjustorThunk','_BlobAssetReference_1_Dispose_m2386336F3AD247A53C738CC3B45803A7D63993D4_AdjustorThunk','_BlobAssetReference_1_Dispose_m8A38672C23BA8BBC3C02C378D8E92E07AAE808A5_AdjustorThunk','_DestroyChunks_Execute_m8FEBFC73937CCF457E24E28BD770BB2212A85E75_AdjustorThunk','_SegmentSortMerge_1_Execute_m853E0FC7F075B850E1FCC2F788F1707E251594DA_AdjustorThunk','_CalculateEntityCountJob_Execute_m5B7C0BED24F44939885B87A902E105D9EC3D7935_AdjustorThunk','_EntityBatchFromEntityChunkDataShared_Execute_m0476C42BCE5BEB4E464E25BBB1AD4EA6FA439323_AdjustorThunk','_ChunkPatchEntities_Execute_mE92FD02568C5805BD9BE232A9C994DE2B238BF74_AdjustorThunk','_MoveAllChunksJob_Execute_mEC08B0343DC7A361EB70673BFD08EA1354D660A0_AdjustorThunk','_MoveChunksBetweenArchetypeJob_Execute_m2AE96A478B19D72A9B06A81C549259D6305362DD_AdjustorThunk','_MoveChunksJob_Execute_m1E6B36786D34534369DBF42D32F252F0127CBB28_AdjustorThunk','_GatherChunksAndOffsetsJob_Execute_m2E05847DA13F1C5BE33ED9A8E897BC76317D6161_AdjustorThunk','_GatherChunksAndOffsetsWithFilteringJob_Execute_m7FE5C03CBEA2953C7C7D9DE554D5605412AC66DC_AdjustorThunk','_FindMissingChild_Execute_m46B9B0202454F0AC4E9211A0EA0CCC089C0533BD_AdjustorThunk','_FixupChangedChildren_Execute_m64311627C1A13D1C8DB90F68B57632036AA8933A_AdjustorThunk','_GatherChildEntities_Execute_m5010D5C102508F8A2F668B294E1A0827606E5808_AdjustorThunk','__ZN2bx16DefaultAllocatorD2Ev','__ZN2bx16DefaultAllocatorD0Ev','__ZN2bx10FileWriterD2Ev','__ZN2bx10FileWriterD0Ev','__ZN2bx10FileWriter5closeEv','__ZThn4_N2bx10FileWriterD1Ev','__ZThn4_N2bx10FileWriterD0Ev','__ZThn4_N2bx10FileWriter5closeEv','__ZThn8_N2bx10FileWriterD1Ev','__ZThn8_N2bx10FileWriterD0Ev','__ZThn12_N2bx10FileWriterD1Ev','__ZThn12_N2bx10FileWriterD0Ev','__ZN4bgfx2gl17RendererContextGLD2Ev','__ZN4bgfx2gl17RendererContextGLD0Ev','__ZN4bgfx2gl17RendererContextGL4flipEv','__ZN4bgfx2gl17RendererContextGL16updateTextureEndEv','__ZN2bx23StaticMemoryBlockWriterD2Ev','__ZN2bx23StaticMemoryBlockWriterD0Ev','__ZThn4_N2bx23StaticMemoryBlockWriterD1Ev','__ZThn4_N2bx23StaticMemoryBlockWriterD0Ev','__ZN2bx17StaticMemoryBlockD2Ev','__ZN2bx17StaticMemoryBlockD0Ev','__ZN2bx13WriterSeekerID2Ev','__ZN2bx11SizerWriterD0Ev','__ZThn4_N2bx11SizerWriterD1Ev','__ZThn4_N2bx11SizerWriterD0Ev','__ZN2bx13ReaderSeekerID2Ev','__ZN2bx12MemoryReaderD0Ev','__ZThn4_N2bx12MemoryReaderD1Ev','__ZThn4_N2bx12MemoryReaderD0Ev','__ZNSt3__214__shared_countD2Ev','__ZNSt3__220__shared_ptr_pointerIPN10astc_codec12_GLOBAL__N_118BitQuantizationMapILi6EEENS_14default_deleteIS4_EENS_9allocatorIS4_EEED0Ev','__ZNSt3__220__shared_ptr_pointerIPN10astc_codec12_GLOBAL__N_118BitQuantizationMapILi6EEENS_14default_deleteIS4_EENS_9allocatorIS4_EEE16__on_zero_sharedEv','__ZNSt3__220__shared_ptr_pointerIPN10astc_codec12_GLOBAL__N_118BitQuantizationMapILi6EEENS_14default_deleteIS4_EENS_9allocatorIS4_EEE21__on_zero_shared_weakEv','__ZNSt3__220__shared_ptr_pointerIPN10astc_codec12_GLOBAL__N_119TritQuantizationMapIXadL_ZNS2_24GetUnquantizedTritWeightEiiiEEEENS_14default_deleteIS4_EENS_9allocatorIS4_EEED0Ev','__ZNSt3__220__shared_ptr_pointerIPN10astc_codec12_GLOBAL__N_119TritQuantizationMapIXadL_ZNS2_24GetUnquantizedTritWeightEiiiEEEENS_14default_deleteIS4_EENS_9allocatorIS4_EEE16__on_zero_sharedEv','__ZNSt3__220__shared_ptr_pointerIPN10astc_codec12_GLOBAL__N_119TritQuantizationMapIXadL_ZNS2_24GetUnquantizedTritWeightEiiiEEEENS_14default_deleteIS4_EENS_9allocatorIS4_EEE21__on_zero_shared_weakEv','__ZNSt3__220__shared_ptr_pointerIPN10astc_codec12_GLOBAL__N_120QuintQuantizationMapIXadL_ZNS2_25GetUnquantizedQuintWeightEiiiEEEENS_14default_deleteIS4_EENS_9allocatorIS4_EEED0Ev','__ZNSt3__220__shared_ptr_pointerIPN10astc_codec12_GLOBAL__N_120QuintQuantizationMapIXadL_ZNS2_25GetUnquantizedQuintWeightEiiiEEEENS_14default_deleteIS4_EENS_9allocatorIS4_EEE16__on_zero_sharedEv','__ZNSt3__220__shared_ptr_pointerIPN10astc_codec12_GLOBAL__N_120QuintQuantizationMapIXadL_ZNS2_25GetUnquantizedQuintWeightEiiiEEEENS_14default_deleteIS4_EENS_9allocatorIS4_EEE21__on_zero_shared_weakEv','__ZNSt3__220__shared_ptr_pointerIPN10astc_codec12_GLOBAL__N_119TritQuantizationMapIXadL_ZNS2_23GetUnquantizedTritValueEiiiEEEENS_14default_deleteIS4_EENS_9allocatorIS4_EEED0Ev','__ZNSt3__220__shared_ptr_pointerIPN10astc_codec12_GLOBAL__N_119TritQuantizationMapIXadL_ZNS2_23GetUnquantizedTritValueEiiiEEEENS_14default_deleteIS4_EENS_9allocatorIS4_EEE16__on_zero_sharedEv','__ZNSt3__220__shared_ptr_pointerIPN10astc_codec12_GLOBAL__N_119TritQuantizationMapIXadL_ZNS2_23GetUnquantizedTritValueEiiiEEEENS_14default_deleteIS4_EENS_9allocatorIS4_EEE21__on_zero_shared_weakEv','__ZNSt3__220__shared_ptr_pointerIPN10astc_codec12_GLOBAL__N_118BitQuantizationMapILi8EEENS_14default_deleteIS4_EENS_9allocatorIS4_EEED0Ev','__ZNSt3__220__shared_ptr_pointerIPN10astc_codec12_GLOBAL__N_118BitQuantizationMapILi8EEENS_14default_deleteIS4_EENS_9allocatorIS4_EEE16__on_zero_sharedEv','__ZNSt3__220__shared_ptr_pointerIPN10astc_codec12_GLOBAL__N_118BitQuantizationMapILi8EEENS_14default_deleteIS4_EENS_9allocatorIS4_EEE21__on_zero_shared_weakEv','__ZNSt3__220__shared_ptr_pointerIPN10astc_codec12_GLOBAL__N_120QuintQuantizationMapIXadL_ZNS2_24GetUnquantizedQuintValueEiiiEEEENS_14default_deleteIS4_EENS_9allocatorIS4_EEED0Ev','__ZNSt3__220__shared_ptr_pointerIPN10astc_codec12_GLOBAL__N_120QuintQuantizationMapIXadL_ZNS2_24GetUnquantizedQuintValueEiiiEEEENS_14default_deleteIS4_EENS_9allocatorIS4_EEE16__on_zero_sharedEv','__ZNSt3__220__shared_ptr_pointerIPN10astc_codec12_GLOBAL__N_120QuintQuantizationMapIXadL_ZNS2_24GetUnquantizedQuintValueEiiiEEEENS_14default_deleteIS4_EENS_9allocatorIS4_EEE21__on_zero_shared_weakEv','__ZN2bx7ReaderID2Ev','__ZN4bgfx2gl10LineReaderD0Ev','__ZN2bx14FileWriterImplD2Ev','__ZN2bx14FileWriterImplD0Ev','__ZN2bx14FileWriterImpl5closeEv','__ZThn4_N2bx14FileWriterImplD1Ev','__ZThn4_N2bx14FileWriterImplD0Ev','__ZThn4_N2bx14FileWriterImpl5closeEv','__ZThn8_N2bx14FileWriterImplD1Ev','__ZThn8_N2bx14FileWriterImplD0Ev','__ZThn12_N2bx14FileWriterImplD1Ev','__ZThn12_N2bx14FileWriterImplD0Ev','__ZN4bgfx16RendererContextID2Ev','__ZN4bgfx4noop19RendererContextNOOPD0Ev','__ZN4bgfx4noop19RendererContextNOOP4flipEv','__ZN4bgfx4noop19RendererContextNOOP16updateTextureEndEv','__ZN2bx10AllocatorID2Ev','__ZN4bgfx13AllocatorStubD0Ev','__ZN4bgfx9CallbackID2Ev','__ZN4bgfx12CallbackStubD0Ev','__ZN4bgfx12CallbackStub11profilerEndEv','__ZN4bgfx12CallbackStub10captureEndEv','__ZN4bgfx11CallbackC99D0Ev','__ZN4bgfx11CallbackC9911profilerEndEv','__ZN4bgfx11CallbackC9910captureEndEv','__ZN4bgfx12AllocatorC99D0Ev','__ZN10__cxxabiv116__shim_type_infoD2Ev','__ZN10__cxxabiv117__class_type_infoD0Ev','__ZNK10__cxxabiv116__shim_type_info5noop1Ev','__ZNK10__cxxabiv116__shim_type_info5noop2Ev','__ZN10__cxxabiv120__si_class_type_infoD0Ev','_JobChunk_Process_1_ProducerCleanupFn_Gen_m6777ACDBA619B6A15731E37C1F5C0F30640EF40D','_ReversePInvokeWrapper_JobChunk_Process_1_ProducerCleanupFn_Gen_m6777ACDBA619B6A15731E37C1F5C0F30640EF40D','_JobChunk_Process_1_ProducerCleanupFn_Gen_m2EDA9936FE94414C48534923E02C921E8E7AA77A','_ReversePInvokeWrapper_JobChunk_Process_1_ProducerCleanupFn_Gen_m2EDA9936FE94414C48534923E02C921E8E7AA77A','_JobChunk_Process_1_ProducerCleanupFn_Gen_mFB728A2E2AE5B6CDBB0E27B81FACD7572C111CA5','_ReversePInvokeWrapper_JobChunk_Process_1_ProducerCleanupFn_Gen_mFB728A2E2AE5B6CDBB0E27B81FACD7572C111CA5','_JobChunk_Process_1_ProducerCleanupFn_Gen_mE41AD4DA49C6F73ACF7C39103885F3AA4846AA2F','_ReversePInvokeWrapper_JobChunk_Process_1_ProducerCleanupFn_Gen_mE41AD4DA49C6F73ACF7C39103885F3AA4846AA2F','_JobChunk_Process_1_ProducerCleanupFn_Gen_m2E35C9A3C9A7B405FD495E3E16544522DB6F0B33','_ReversePInvokeWrapper_JobChunk_Process_1_ProducerCleanupFn_Gen_m2E35C9A3C9A7B405FD495E3E16544522DB6F0B33','_JobChunk_Process_1_ProducerCleanupFn_Gen_m64EADC0BD437BAE642E416B3E2E33255850D804A','_ReversePInvokeWrapper_JobChunk_Process_1_ProducerCleanupFn_Gen_m64EADC0BD437BAE642E416B3E2E33255850D804A','_JobChunk_Process_1_ProducerCleanupFn_Gen_mBE9428339C1A497440977BB9B161103777DB6154','_ReversePInvokeWrapper_JobChunk_Process_1_ProducerCleanupFn_Gen_mBE9428339C1A497440977BB9B161103777DB6154','_JobChunk_Process_1_ProducerCleanupFn_Gen_mA6074044FC5C8044F5736833F773A1C875BF6F7E','_ReversePInvokeWrapper_JobChunk_Process_1_ProducerCleanupFn_Gen_mA6074044FC5C8044F5736833F773A1C875BF6F7E','_JobChunk_Process_1_ProducerCleanupFn_Gen_m047B4E81A669E56BC7C80BA73EDB61B35AEAFB4A','_ReversePInvokeWrapper_JobChunk_Process_1_ProducerCleanupFn_Gen_m047B4E81A669E56BC7C80BA73EDB61B35AEAFB4A','_JobChunk_Process_1_ProducerCleanupFn_Gen_m352E93F07A32882E32ED52B50FDADF61BA2BBE2A','_ReversePInvokeWrapper_JobChunk_Process_1_ProducerCleanupFn_Gen_m352E93F07A32882E32ED52B50FDADF61BA2BBE2A','_JobChunk_Process_1_ProducerCleanupFn_Gen_mEFF9FE27C10151F6A7BE27CEFC250150977A85E3','_ReversePInvokeWrapper_JobChunk_Process_1_ProducerCleanupFn_Gen_mEFF9FE27C10151F6A7BE27CEFC250150977A85E3','_JobChunk_Process_1_ProducerCleanupFn_Gen_m37F2673249591B57C244D379ADB85ABBC3CC2CFB','_ReversePInvokeWrapper_JobChunk_Process_1_ProducerCleanupFn_Gen_m37F2673249591B57C244D379ADB85ABBC3CC2CFB','_JobChunk_Process_1_ProducerCleanupFn_Gen_m7320113749E95A876E039F48FBD9179EB227DC70','_ReversePInvokeWrapper_JobChunk_Process_1_ProducerCleanupFn_Gen_m7320113749E95A876E039F48FBD9179EB227DC70','_JobChunk_Process_1_ProducerCleanupFn_Gen_mD1E3B491F8993A9DE549EA484BB9BAD80CF6FEA6','_ReversePInvokeWrapper_JobChunk_Process_1_ProducerCleanupFn_Gen_mD1E3B491F8993A9DE549EA484BB9BAD80CF6FEA6','_JobChunk_Process_1_ProducerCleanupFn_Gen_mB25E482F8BF0799DDBEC2DF1B5376FE226FC6A32','_ReversePInvokeWrapper_JobChunk_Process_1_ProducerCleanupFn_Gen_mB25E482F8BF0799DDBEC2DF1B5376FE226FC6A32','_JobChunk_Process_1_ProducerCleanupFn_Gen_m01A280AA72A195C57733C63531E2A4EE64025B6C','_ReversePInvokeWrapper_JobChunk_Process_1_ProducerCleanupFn_Gen_m01A280AA72A195C57733C63531E2A4EE64025B6C','_JobChunk_Process_1_ProducerCleanupFn_Gen_m20D20DCFA71B327BE2AA3383CF80BF03B4C65050','_ReversePInvokeWrapper_JobChunk_Process_1_ProducerCleanupFn_Gen_m20D20DCFA71B327BE2AA3383CF80BF03B4C65050','_JobChunk_Process_1_ProducerCleanupFn_Gen_m9A4D5736129B8C258FB580E8424C763EAE7EF6D0','_ReversePInvokeWrapper_JobChunk_Process_1_ProducerCleanupFn_Gen_m9A4D5736129B8C258FB580E8424C763EAE7EF6D0','_JobChunk_Process_1_ProducerCleanupFn_Gen_m56552195A0779E150DA88EAF890634E13C1134F9','_ReversePInvokeWrapper_JobChunk_Process_1_ProducerCleanupFn_Gen_m56552195A0779E150DA88EAF890634E13C1134F9','_JobChunk_Process_1_ProducerCleanupFn_Gen_m1BD792634E2F5C8157F8FA6619BB74EA8865F1DD','_ReversePInvokeWrapper_JobChunk_Process_1_ProducerCleanupFn_Gen_m1BD792634E2F5C8157F8FA6619BB74EA8865F1DD','_JobChunk_Process_1_ProducerCleanupFn_Gen_m21086F2B1D3E1D6658547EE85B22FCA496AE4284','_ReversePInvokeWrapper_JobChunk_Process_1_ProducerCleanupFn_Gen_m21086F2B1D3E1D6658547EE85B22FCA496AE4284','_JobChunk_Process_1_ProducerCleanupFn_Gen_mAF7FBCAD884197CF5C78231F2515AD9E7DBD33AB','_ReversePInvokeWrapper_JobChunk_Process_1_ProducerCleanupFn_Gen_mAF7FBCAD884197CF5C78231F2515AD9E7DBD33AB','_JobChunk_Process_1_ProducerCleanupFn_Gen_m58F67B4C4A5E71EE6D3BCF680BD08E000A095195','_ReversePInvokeWrapper_JobChunk_Process_1_ProducerCleanupFn_Gen_m58F67B4C4A5E71EE6D3BCF680BD08E000A095195','_ParallelForJobStruct_1_ProducerCleanupFn_Gen_mD2D2544FA11E9BD5699AFC7A5F0D070EF0D75A28','_ReversePInvokeWrapper_ParallelForJobStruct_1_ProducerCleanupFn_Gen_mD2D2544FA11E9BD5699AFC7A5F0D070EF0D75A28','_ParallelForJobStruct_1_ProducerCleanupFn_Gen_m80FFED589098020394C2357B759C6923185715BF','_ReversePInvokeWrapper_ParallelForJobStruct_1_ProducerCleanupFn_Gen_m80FFED589098020394C2357B759C6923185715BF','_ParallelForJobStruct_1_ProducerCleanupFn_Gen_m7A4A1C3F7F21092B8F829E38FE713B661AECABBB','_ReversePInvokeWrapper_ParallelForJobStruct_1_ProducerCleanupFn_Gen_m7A4A1C3F7F21092B8F829E38FE713B661AECABBB','_ParallelForJobStruct_1_ProducerCleanupFn_Gen_m488F9A63BDDFDF1B3FB6792A10CCBF3C7EBA5996','_ReversePInvokeWrapper_ParallelForJobStruct_1_ProducerCleanupFn_Gen_m488F9A63BDDFDF1B3FB6792A10CCBF3C7EBA5996','_ParallelForJobStruct_1_ProducerCleanupFn_Gen_mEA16758F97B5EC6DCE3A6A680A3280686D0405C8','_ReversePInvokeWrapper_ParallelForJobStruct_1_ProducerCleanupFn_Gen_mEA16758F97B5EC6DCE3A6A680A3280686D0405C8','_ParallelForJobStruct_1_ProducerCleanupFn_Gen_mC161D54DE2EB3D828E0FAC7533A5B0EFA0C0AF3B','_ReversePInvokeWrapper_ParallelForJobStruct_1_ProducerCleanupFn_Gen_mC161D54DE2EB3D828E0FAC7533A5B0EFA0C0AF3B','_ParallelForJobStruct_1_ProducerCleanupFn_Gen_m912641F0083FF7DD8FE8A7ECEE9DC73112ED6107','_ReversePInvokeWrapper_ParallelForJobStruct_1_ProducerCleanupFn_Gen_m912641F0083FF7DD8FE8A7ECEE9DC73112ED6107','_ParallelForJobStruct_1_ProducerCleanupFn_Gen_m38833EE20E53A61C11E3E4F6480827058355FD5A','_ReversePInvokeWrapper_ParallelForJobStruct_1_ProducerCleanupFn_Gen_m38833EE20E53A61C11E3E4F6480827058355FD5A','_GC_null_finalize_mark_proc','_GC_unreachable_finalize_mark_proc','__ZL12profiler_endP25bgfx_callback_interface_s','__ZL11capture_endP25bgfx_callback_interface_s','__ZN5Unity4Tiny2IOL9OnSuccessEP18emscripten_fetch_t','__ZN5Unity4Tiny2IOL7OnErrorEP18emscripten_fetch_t','_emscripten_glActiveTexture','_emscripten_glBlendEquation','_emscripten_glClear','_emscripten_glClearStencil','_emscripten_glCompileShader','_emscripten_glCullFace','_emscripten_glDeleteProgram','_emscripten_glDeleteShader','_emscripten_glDepthFunc','_emscripten_glDepthMask','_emscripten_glDisable','_emscripten_glDisableVertexAttribArray','_emscripten_glEnable','_emscripten_glEnableVertexAttribArray','_emscripten_glFrontFace','_emscripten_glGenerateMipmap','_emscripten_glLinkProgram','_emscripten_glStencilMask','_emscripten_glUseProgram','_emscripten_glValidateProgram','_emscripten_glEndQueryEXT','_emscripten_glBindVertexArrayOES','_emscripten_glReadBuffer','_emscripten_glEndQuery','_emscripten_glBindVertexArray','_emscripten_glBeginTransformFeedback','_emscripten_glDeleteSync','__ZN10__cxxabiv112_GLOBAL__N_19destruct_EPv',0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0];
var debug_table_vif = [0,'_emscripten_glUniform1f$legalf32','_emscripten_glVertexAttrib1f$legalf32',0];
var debug_table_viff = [0,'_emscripten_glUniform2f$legalf32','_emscripten_glVertexAttrib2f$legalf32',0];
var debug_table_vifff = [0,'_emscripten_glUniform3f$legalf32','_emscripten_glVertexAttrib3f$legalf32',0];
var debug_table_viffff = [0,'_emscripten_glUniform4f$legalf32','_emscripten_glVertexAttrib4f$legalf32',0];
var debug_table_vii = [0,'_F_E_Invoke_m1E7D15AD6038858E0705F85E5F4E61FD668D0A73','_ComponentSystem_OnBeforeCreateInternal_m04C4BDD690DDEA9E8525ED88B2829A659598CA21','_ComponentSystemBase_OnBeforeCreateInternal_mCDC97E13CEBE29CDC67589D3616B3CB74C0C232A','_JobComponentSystem_OnBeforeCreateInternal_mE65CEE7ABE4CBB948AD5FE9FE467689ABD2DF104','_AudioHTMLSystem_StopSource_m34F75E83F3259FD7BB1ECB3EC2EBA687B1DCC018','_F_D_1_Invoke_mB83660AC0C27D04DFD699BBD00B9CCE918BCFF7F','_F_D_1_Invoke_m38133B8521D1503BCE23CD4D44D94CEFA2F6A153','_F_D_1_Invoke_m2F895DEC98C28FB956766A1B5CD02A014B8C75C4','_Enumerator_get_Current_mB673C6AF7DFEF98F376873100E0238C2DF9B4FAA_AdjustorThunk','_Enumerator_get_Current_m9233F1071FB58219970A54AEC18E10143BF40E3E_AdjustorThunk','_Enumerator_get_Current_mB6D8BA6C5004B605B795CB47A58FE5C6DAAC8B82_AdjustorThunk','_Enumerator_get_Current_m53F7CAA50D8BB492C704D410A43FACBF441DB673_AdjustorThunk','_Enumerator_get_Current_m4A2C194788C4115A29B385F3B66679FE3EC27EBF_AdjustorThunk','_Enumerator_get_Current_mD965D5E53C01251F5386669028DA231A4FBB922B_AdjustorThunk','_Enumerator_get_Current_mB35A14BCCC37282A70DEB5CB32DFE59973BEBAED_AdjustorThunk','_Enumerator_get_Current_m2B61CDFBEF92E943F3E8CF8DD77670825659F187_AdjustorThunk','_Enumerator_get_Current_mB39E22E8C768073B595B69B4CA7B5D7EB1C1D1E3_AdjustorThunk','_Enumerator_get_Current_m0BAEEBC4AC7B59D89535F618DDEF5C8C43931216_AdjustorThunk','_Enumerator_get_Current_mA3FD3B5A19E1E743484F98D0BF9377F58638250C_AdjustorThunk','_Enumerator_get_Current_m79FA1C20E3C5D331C76D92A05403F46D9D41C1A3_AdjustorThunk','_Enumerator_get_Current_m3CC7B9372A68E00C4C76D3388BE72D3946CB524B_AdjustorThunk','_Enumerator_get_Current_m005980142162981DCDD94D83C2AAEFC118605CF2_AdjustorThunk','_Enumerator_get_Current_m46F32FC8FE620261158174DA66AD92295469CD68_AdjustorThunk','_Enumerator_get_Current_m57E54536866A26D05382677771AD7500F5604C78_AdjustorThunk','_Enumerator_get_Current_m3610BE94BC51814051AF6239260A4B5E7AFFA9F1_AdjustorThunk','_Enumerator_get_Current_m85B1AD8AEF70251CA3648D40365528D0AA801683_AdjustorThunk','_Enumerator_get_Current_mD9162870416117B1985E16301CBB787FDF323900_AdjustorThunk','_Enumerator_get_Current_m974B8AFC234BD8A39FDC0B3E96330DEB313C2DCE_AdjustorThunk','_Enumerator_get_Current_m4D0498C25809D5EA48B32B83C0A4F97CD2DD036B_AdjustorThunk','_Enumerator_get_Current_mCA9A112B13D58905777AF039050DD00A13CACE7E_AdjustorThunk','_Enumerator_get_Current_mEF1DDCC1B602D232991900CEDFA7DF4C3082B82A_AdjustorThunk','_Enumerator_get_Current_mC39DF6902E5EA0B1A240ECBC8B6BD59213D46C6E_AdjustorThunk','_Enumerator_get_Current_m6216DC72D5F3D3F958C1E5BFBE42349BD3CCEBC2_AdjustorThunk','_Enumerator_get_Current_m90006F5F360DE3031520BBD5F842DE851EEE1E68_AdjustorThunk','_Enumerator_get_Current_m9F1EE2D839F84A7EC125242D174A386A65D5F008_AdjustorThunk','_Enumerator_get_Current_m70496A5F65B3E4FD2F381A90A6F46D318015308F_AdjustorThunk','_Enumerator_get_Current_mC2DA54492FCFB9E11AE1323B8EB935F2DC8897C2_AdjustorThunk','_Enumerator_get_Current_mDC8AE8CC530943DCF3DF1D5B9804F6BDDC9AF775_AdjustorThunk','_Enumerator_get_Current_m4269772B3E506FE2D936426F7E3E6056BFE6ADED_AdjustorThunk','_Enumerator_get_Current_m36D3E55B2A316ACFC6AA16798993C8B85E2AE13B_AdjustorThunk','_Enumerator_get_Current_mA9C611D911163CE336E06D652EBB8105B0A707DE_AdjustorThunk','_Enumerator_get_Current_m050E79092E7623DF589E552A30C4DBE77C493068_AdjustorThunk','_Enumerator_get_Current_mD3F7B5DAF11CFD6AFA4D834D9988374DA191D106_AdjustorThunk','_Enumerator_get_Current_mA5F276894F86DD136AD3D64EDF9FB80FA285C7F2_AdjustorThunk','_Enumerator_get_Current_mC5D6C6B0DAF51A3949293651CA6A90D8C6E41032_AdjustorThunk','_Enumerator_get_Current_m9F39FAAE9330ACE3B24F9CEE66662E85C76DB330_AdjustorThunk','_Enumerator_get_Current_m98FB598272CD9F4288CF03A41E777764EE441DFB_AdjustorThunk','_Enumerator_get_Current_m436F0B1F50C492A99EBF1FAF175D9C00D5440472_AdjustorThunk','_Enumerator_get_Current_m02EF5A06515A0A332DA0EEA67B79F326B81AECB1_AdjustorThunk','_Enumerator_get_Current_m87DE5502009935E2BB863445A645FB22415AC26E_AdjustorThunk','_Enumerator_get_Current_mF8B32694F6ABF4E149A7DCDB8E004046EA3C9C6D_AdjustorThunk','_Enumerator_get_Current_mE715F3216FD4034E181543E779C8FA68C9F78118_AdjustorThunk','_Enumerator_get_Current_mF1EF3A87E58D77EA85C3068447F3BDFAAD3D06E8_AdjustorThunk','_Enumerator_get_Current_m36C9C5B06E431C1E01A0522A13453D077F14BBDC_AdjustorThunk','_Enumerator_get_Current_m92D1568824ABE4D08A4F618575167EC5762D9F0F_AdjustorThunk','_Enumerator_get_Current_m9063343482C1E665CC99DA4018F4D3B3CE82EAEE_AdjustorThunk','_Enumerator_get_Current_mA07FC0584A5508254E192E2D8A77627D840C3345_AdjustorThunk','_Enumerator_get_Current_m1F59D71D0FCF83D8244DA0E0DF5638F095578E94_AdjustorThunk','_Enumerator_get_Current_mCA489114DCF6DD1B7EDC284FC65F225C1B835A82_AdjustorThunk','_Enumerator_get_Current_m30D2AD480B32BE4AC799BAC4B82CE8710D98240D_AdjustorThunk','_Enumerator_get_Current_m406B0B85DF81AA9206E41692B7639BB5AE91B626_AdjustorThunk','_Enumerator_get_Current_m376264075D8BF7FA32AC98E6991B1FDAABE0238A_AdjustorThunk','_Enumerator_get_Current_mD5FDAC2DB43E5BF3636928AA8C7805875FD50921_AdjustorThunk','_Enumerator_get_Current_m50592DB23129A2F6E5D0C6A144858310EBD7FCE9_AdjustorThunk','_Enumerator_get_Current_mC60C108D38BBFB0CE355E93907A9F5A50BAF8D3C_AdjustorThunk','_Enumerator_get_Current_m1C5095A1C352ACE05F09ACD13283E6DA5F1BEBF3_AdjustorThunk','_Enumerator_get_Current_mA297F24C01DB006870BD5C41ED796D59DE3EAE9A_AdjustorThunk','_Enumerator_get_Current_mE24A39A9222208CBA9A791949086CB722713ECDC_AdjustorThunk','_Enumerator_get_Current_m3D4C63CE52E1D170DA7C6E1F2CA7BA066C1A74E9_AdjustorThunk','_Enumerator_get_Current_m79FB9C24C2E944533A1C06DAFF09CCAF7E79D6AE_AdjustorThunk','_Enumerator_get_Current_mACD5C642BFE7BE06158518877AE6557202FAC950_AdjustorThunk','_Enumerator_get_Current_m2064EDB25837BA0B591EA158F6A8B73583391DDB_AdjustorThunk','_Enumerator_get_Current_mD076220C2ABF1BBA1DF6908678893AC068FFA739_AdjustorThunk','_Enumerator_get_Current_mD21DBA1CEEBC24FBF54A7C0AA1AEB872C39E36B8_AdjustorThunk','_Enumerator_get_Current_m4AC305A341B77F6406206BA4A2CA6742AC66B553_AdjustorThunk','_Enumerator_get_Current_m3FD561ADE5D8AA231171A36A652EC97EBEBFBFB9_AdjustorThunk','_Enumerator_get_Current_m79C56D27791753AD5CE4EC9DCCD913FD8EE25FDB_AdjustorThunk','_Enumerator_get_Current_m8A60154652BFE6132280E8C9FAA4D6A660795F44_AdjustorThunk','_Enumerator_get_Current_m493A10BE509CB37E12560D99DE5C4AF0969E9BDE_AdjustorThunk','_Enumerator_get_Current_m8A0D073EAFB8608673214C51323BE8490ABFD9DE_AdjustorThunk','_Enumerator_get_Current_m1B5144ED49D61E3C4C23DC87E5AF4AD2895FC751_AdjustorThunk','_Enumerator_get_Current_m0E818D3B17385E7DFA9A16E371B0BA028C7A71CC_AdjustorThunk','_Enumerator_get_Current_m634EEBB1F0AA8A7E7DFAA2B84A2A68CAAA4DA717_AdjustorThunk','_U3CU3Ec__DisplayClass_OnUpdate_LambdaJob0_PrepareJobAtExecuteTimeFn_Gen_m37280AB3D73296DAD59F5A6C8C449DEACA8B90E6_AdjustorThunk','_U3CU3Ec__DisplayClass_OnUpdate_LambdaJob0_CleanupJobFn_Gen_m5B94B25F46F26F2B1DC44150A7C0C37D88EBB32B_AdjustorThunk','_U3CU3Ec__DisplayClass_OnUpdate_LambdaJob0_ReadFromDisplayClass_m8C3228A9E0FB36AB8C92D6A8F99C364C534820FF_AdjustorThunk','_ManagedJobDelegate_Invoke_m6928DC001962A045DE74B7F1310D972FE3A7696F','_U3CU3Ec__DisplayClass_OnUpdate_LambdaJob0_PrepareJobAtExecuteTimeFn_Gen_mFBACD3729D10E41FA5E68A0D5ECEF719E1F476E6_AdjustorThunk','_U3CU3Ec__DisplayClass_OnUpdate_LambdaJob0_CleanupJobFn_Gen_mC1B9696592C3AA68B63B78941B30F177B03BDB0C_AdjustorThunk','_U3CU3Ec__DisplayClass_OnUpdate_LambdaJob0_ReadFromDisplayClass_mA2F76FF6AC464CC7BFC78568B6131F81B079459E_AdjustorThunk','_U3CU3Ec__DisplayClass_OnUpdate_LambdaJob0_PrepareJobAtExecuteTimeFn_Gen_mE2414AB3390A0431A76FBBFC6EF13865C37559BA_AdjustorThunk','_U3CU3Ec__DisplayClass_OnUpdate_LambdaJob0_CleanupJobFn_Gen_mF9823F078429A56228B9CF32BADC2CA0180277FB_AdjustorThunk','_U3CU3Ec__DisplayClass_OnUpdate_LambdaJob0_ReadFromDisplayClass_m7BA67AF86D9E7EDAFC7DBF3EBC5D6022B19BF55C_AdjustorThunk','_U3CU3Ec__DisplayClass_OnUpdate_LambdaJob0_PrepareJobAtExecuteTimeFn_Gen_m5237DD983C74AD2FDEE8481EDD4B43495A515466_AdjustorThunk','_U3CU3Ec__DisplayClass_OnUpdate_LambdaJob0_CleanupJobFn_Gen_mA410DD4BE59FE8B70ABAB9E4952E72B537C77E56_AdjustorThunk','_U3CU3Ec__DisplayClass_OnUpdate_LambdaJob0_ReadFromDisplayClass_mA312FFB7EBC2D506A73852C74E7D1F83D3A285AA_AdjustorThunk','_U3CU3Ec__DisplayClass_OnUpdate_LambdaJob0_PrepareJobAtExecuteTimeFn_Gen_mC51ED8C01C689D560D1E5CC1301FED87FFC14D1E_AdjustorThunk','_U3CU3Ec__DisplayClass_OnUpdate_LambdaJob0_CleanupJobFn_Gen_m611B1AE424222BC8E8A0205979CB0C02DA3D2345_AdjustorThunk','_U3CU3Ec__DisplayClass_OnUpdate_LambdaJob0_ReadFromDisplayClass_mFC8B340062C9DD0F74576C30EB1957FCE9EFBEFC_AdjustorThunk','_U3CU3Ec__DisplayClass_OnUpdate_LambdaJob0_PrepareJobAtExecuteTimeFn_Gen_mA0D213903C6A3CAA9D00A69CB2318EB3C4FCB8C3_AdjustorThunk','_U3CU3Ec__DisplayClass_OnUpdate_LambdaJob0_CleanupJobFn_Gen_m7597887897C74DF5FD3666EADDBADAEF4CA693B3_AdjustorThunk','_U3CU3Ec__DisplayClass_OnUpdate_LambdaJob0_ReadFromDisplayClass_m515D131DF8E047F0A868F53544BA0F0AA016143B_AdjustorThunk','_U3CU3Ec__DisplayClass_OnUpdate_LambdaJob0_PrepareJobAtExecuteTimeFn_Gen_m7B7CF8602B8E444DE951438BA19F3D389005C2FD_AdjustorThunk','_U3CU3Ec__DisplayClass_OnUpdate_LambdaJob0_CleanupJobFn_Gen_m6C39D6DE4E947F837F9533DC9F362BF542D4303A_AdjustorThunk','_U3CU3Ec__DisplayClass_OnUpdate_LambdaJob0_ReadFromDisplayClass_mF6DDDA918ECEF8DAB5C225AEEAB8E6A96976CFB9_AdjustorThunk','_U3CU3Ec__DisplayClass_OnUpdate_LambdaJob0_PrepareJobAtExecuteTimeFn_Gen_m3F0298DB7EFC0F904E1EF981DFE543317578E21F_AdjustorThunk','_U3CU3Ec__DisplayClass_OnUpdate_LambdaJob0_CleanupJobFn_Gen_m901C96D0617B9413CCD498FBF803427E46CF50ED_AdjustorThunk','_U3CU3Ec__DisplayClass_OnUpdate_LambdaJob0_ReadFromDisplayClass_m203F4EE908A89DE24D36FCB0B8977BB4F88B22DB_AdjustorThunk','_U3CU3Ec__DisplayClass_OnUpdate_LambdaJob0_PrepareJobAtExecuteTimeFn_Gen_m6F9B3FA1F6BFA110BCFE4B91DBBF125E3C7BF8FB_AdjustorThunk','_U3CU3Ec__DisplayClass_OnUpdate_LambdaJob0_CleanupJobFn_Gen_mEC55CAB3B31E2BE31813F790DCC48A54786B32EC_AdjustorThunk','_U3CU3Ec__DisplayClass_OnUpdate_LambdaJob0_ReadFromDisplayClass_mC036DD62DE2819765C019648CC98445740EF6829_AdjustorThunk','_GatherComponentDataJob_1_PrepareJobAtExecuteTimeFn_Gen_m53226863CD59CCDCEA3275B5E0ED6A8C4F83F6CA_AdjustorThunk','_GatherComponentDataJob_1_CleanupJobFn_Gen_mD552AD9BFC151A00C9A6D5AE35622769D64EA9F6_AdjustorThunk','_GatherEntitiesJob_PrepareJobAtExecuteTimeFn_Gen_m8321C89511307CAC65822ABC980405B441C73122_AdjustorThunk','_GatherEntitiesJob_CleanupJobFn_Gen_mB87C76B7F35C22269354DC1777533B0004A545EC_AdjustorThunk','_SubmitSimpleLitMeshJob_PrepareJobAtExecuteTimeFn_Gen_m07961857B0C4FB88B86994DFCDE83E83EDBF54A2_AdjustorThunk','_SubmitSimpleLitMeshJob_CleanupJobFn_Gen_m25803C3D6CBE879C521272526620087CF39E68F9_AdjustorThunk','_BuildEntityGuidHashMapJob_PrepareJobAtExecuteTimeFn_Gen_m68EE3A5F62CEC38D345E2FFE0DA9F781CD983333_AdjustorThunk','_BuildEntityGuidHashMapJob_CleanupJobFn_Gen_m3BCE259A491B480B2C36101AED5053CB41F6877F_AdjustorThunk','_ToCompositeRotation_PrepareJobAtExecuteTimeFn_Gen_mAC3DB22BE9FACAE2FCC117DFE22094BDFC3D1E63_AdjustorThunk','_ToCompositeRotation_CleanupJobFn_Gen_m8C444F7A430728FA5242691098E0E5DE069EC7C0_AdjustorThunk','_ToCompositeScale_PrepareJobAtExecuteTimeFn_Gen_m7E19B6D81F298B3200298406BC06B99C900A6698_AdjustorThunk','_ToCompositeScale_CleanupJobFn_Gen_mBA9026CBE983CA569495E91E3F9D6D0BB216C6E9_AdjustorThunk','_UpdateHierarchy_PrepareJobAtExecuteTimeFn_Gen_mE5943AA360841797342CC8E8422309E33F92361D_AdjustorThunk','_UpdateHierarchy_CleanupJobFn_Gen_m5419C26A4C7E1F7FE43157EA877E56D2E083405E_AdjustorThunk','_ToChildParentScaleInverse_PrepareJobAtExecuteTimeFn_Gen_mDBA7BC5B07B408C32E62933D8CFCAD2D0C1E11A1_AdjustorThunk','_ToChildParentScaleInverse_CleanupJobFn_Gen_m3D71AB9AB129F0B4760FC87F58685705DA40109F_AdjustorThunk','_GatherChangedParents_PrepareJobAtExecuteTimeFn_Gen_m3ECE0CE3618512A4619CFD6B9863AE21E2A260CF_AdjustorThunk','_GatherChangedParents_CleanupJobFn_Gen_m9C45E9507F766EF820CAD99F0B7B7BAECE5A8A43_AdjustorThunk','_PostRotationEulerToPostRotation_PrepareJobAtExecuteTimeFn_Gen_mED17ECA34F68515DD5E225C82C7F64F11DF8610A_AdjustorThunk','_PostRotationEulerToPostRotation_CleanupJobFn_Gen_m58155B94E373C8BD56F0F73C9228ADFA255B43A5_AdjustorThunk','_RotationEulerToRotation_PrepareJobAtExecuteTimeFn_Gen_mEC8C58D1FE49E7FA5D8594633BFA57D1C3C93805_AdjustorThunk','_RotationEulerToRotation_CleanupJobFn_Gen_mB4336C07420DEB151D73ADB02D768CDDA150E739_AdjustorThunk','_TRSToLocalToParent_PrepareJobAtExecuteTimeFn_Gen_m3BE3C4EDCE5D336B06B2B20994D4FDE213A83B52_AdjustorThunk','_TRSToLocalToParent_CleanupJobFn_Gen_mF57E707E177D37BED2E75C20AFE2E322DD349E02_AdjustorThunk','_TRSToLocalToWorld_PrepareJobAtExecuteTimeFn_Gen_m67AA6DF57D0E5A2D2C7D89522E285C2B527D5D08_AdjustorThunk','_TRSToLocalToWorld_CleanupJobFn_Gen_m88FAD67D9FC2ADF36CABF91C9616D032EE91C947_AdjustorThunk','_ToWorldToLocal_PrepareJobAtExecuteTimeFn_Gen_m2622024B3A7C4AA8BDC92BBD2C7D020D3226A1E4_AdjustorThunk','_ToWorldToLocal_CleanupJobFn_Gen_mEE911E122C0B74532CB81D82563307D793F0FADF_AdjustorThunk','_DestroyChunks_PrepareJobAtExecuteTimeFn_Gen_m9BCE64F53DDDAEFE25ED2B2C15C8F1A2B41EFF1C_AdjustorThunk','_DestroyChunks_CleanupJobFn_Gen_mE15E6F7A36B9DEF010D6DEA4A73274B1A735D11B_AdjustorThunk','_SegmentSortMerge_1_PrepareJobAtExecuteTimeFn_Gen_m4A781B153B2BB10171881F01DC732D6F45A91F20_AdjustorThunk','_SegmentSortMerge_1_CleanupJobFn_Gen_mF26CA26DAF009BEF2BD032098C2E38582AEB49DA_AdjustorThunk','_CalculateEntityCountJob_PrepareJobAtExecuteTimeFn_Gen_m6D2B8EDC6BBDEBA413FE8207478D8844C3455D59_AdjustorThunk','_CalculateEntityCountJob_CleanupJobFn_Gen_m75028A8E1DE744E96DA6C58D080E3CA012A2B9F7_AdjustorThunk','_EntityBatchFromEntityChunkDataShared_PrepareJobAtExecuteTimeFn_Gen_m58F7E83F3B1659BB6DF5D790ABEA064F81A552CA_AdjustorThunk','_EntityBatchFromEntityChunkDataShared_CleanupJobFn_Gen_mD704DB71855D082ED3672D6CE190D00DDDEC1F96_AdjustorThunk','_ChunkPatchEntities_PrepareJobAtExecuteTimeFn_Gen_m3F04BAD84A84519C8F14A70707DF22F99C588AE2_AdjustorThunk','_ChunkPatchEntities_CleanupJobFn_Gen_mAA610413772EBD10919F7FE57629E6ADED6A4EC1_AdjustorThunk','_MoveAllChunksJob_PrepareJobAtExecuteTimeFn_Gen_m4019488A8B9B504872711A7398D16392BBE436FD_AdjustorThunk','_MoveAllChunksJob_CleanupJobFn_Gen_m7A6F013E3D2D5605A5C9AAD2F60AC5FE19A113EA_AdjustorThunk','_MoveChunksBetweenArchetypeJob_PrepareJobAtExecuteTimeFn_Gen_m9AFE1BC3828EBCAB4F922D5264585D23216851D0_AdjustorThunk','_MoveChunksBetweenArchetypeJob_CleanupJobFn_Gen_m1579F5BEAFECB525F7B0388E1EE99B32E04AA9CD_AdjustorThunk','_MoveChunksJob_PrepareJobAtExecuteTimeFn_Gen_m03FDC93253D4A23034577BAFD86BD4328D31B56E_AdjustorThunk','_MoveChunksJob_CleanupJobFn_Gen_m95A90FAE42B9ECBC428528B7F6466B7A40F8621E_AdjustorThunk','_GatherChunksAndOffsetsJob_PrepareJobAtExecuteTimeFn_Gen_mD723F76E7065D2118344AEDDC97489851F70C229_AdjustorThunk','_GatherChunksAndOffsetsJob_CleanupJobFn_Gen_mBACC1F9BEA35956913391CFE7F4EA91B62BDB0E5_AdjustorThunk','_GatherChunksAndOffsetsWithFilteringJob_PrepareJobAtExecuteTimeFn_Gen_mD3C9C311F36D4709F5B1ADF6744EE756F09CE2A8_AdjustorThunk','_GatherChunksAndOffsetsWithFilteringJob_CleanupJobFn_Gen_m94A007C00D602E79DCF850536F79E85D2B5C9DB7_AdjustorThunk','_FindMissingChild_PrepareJobAtExecuteTimeFn_Gen_mA48763120267CBA1130396E3046F22C92B920C49_AdjustorThunk','_FindMissingChild_CleanupJobFn_Gen_mDD4625BD72FC433C1C606B881D547F857D93344D_AdjustorThunk','_FixupChangedChildren_PrepareJobAtExecuteTimeFn_Gen_mEDC50C3AFD5D4FCFD83991028847D57AE69821C5_AdjustorThunk','_FixupChangedChildren_CleanupJobFn_Gen_mD1CAFA1732DC079B30F0E174F7319C6912C86C31_AdjustorThunk','_GatherChildEntities_PrepareJobAtExecuteTimeFn_Gen_m00A8FD5008F30DAA33B623D408461931A8326DB6_AdjustorThunk','_GatherChildEntities_CleanupJobFn_Gen_m8E2A880EBF87CAF9725B87CF72DF8C324BF4935A_AdjustorThunk','_SegmentSort_1_Execute_m5F0D1D64BE1DE540CE0DBE1B64C60B166A1203E2_AdjustorThunk','_SegmentSort_1_PrepareJobAtExecuteTimeFn_Gen_m5D0D27EC4DF321BA55D44D07C631B861CF677013_AdjustorThunk','_SegmentSort_1_CleanupJobFn_Gen_mA8D35FC6F40E3E0D1860513E3AE90EF5A84B8682_AdjustorThunk','_GatherEntityInChunkForEntities_Execute_mD9F62BBDE672B6639B65B54A09C90001351F07BE_AdjustorThunk','_GatherEntityInChunkForEntities_PrepareJobAtExecuteTimeFn_Gen_m4A0F3CCF1D445A20D727CF6DB640EDEE7ADDE6B1_AdjustorThunk','_GatherEntityInChunkForEntities_CleanupJobFn_Gen_m8A25DBAE48B79060585AE4209923064722D509BA_AdjustorThunk','_RemapAllChunksJob_Execute_mB2A2BDBA45FFBDD48D00F625CD1E2CF288FEFDAB_AdjustorThunk','_RemapAllChunksJob_PrepareJobAtExecuteTimeFn_Gen_m69EA91E200D18F4677E5ED226151BBBDA3471587_AdjustorThunk','_RemapAllChunksJob_CleanupJobFn_Gen_m8B885C2193CA026F63D555B60A6C25E61065CA4C_AdjustorThunk','_RemapArchetypesJob_Execute_m66BC5AC93EE6024E5F1EE43250D479AB360B789F_AdjustorThunk','_RemapArchetypesJob_PrepareJobAtExecuteTimeFn_Gen_mD6FA7D6AB5B0B0D9751F22449756DF896AFC6961_AdjustorThunk','_RemapArchetypesJob_CleanupJobFn_Gen_m08C5FD0047236DFDBD5252AEFF4DFBF1D9AAD393_AdjustorThunk','_RemapChunksJob_Execute_m50B31B16DBC304FF1E9BB488F325B0A0CDC551A3_AdjustorThunk','_RemapChunksJob_PrepareJobAtExecuteTimeFn_Gen_m241D311601EEA0BC1A78D4EDDE27B9E44C65FBE1_AdjustorThunk','_RemapChunksJob_CleanupJobFn_Gen_m93D78E6B53BFE88516CC178DB15D5FA35F24B5BC_AdjustorThunk','_RemapManagedArraysJob_Execute_m1E359E03140722B1FB8E6473DB799334C7017A41_AdjustorThunk','_RemapManagedArraysJob_PrepareJobAtExecuteTimeFn_Gen_mDE6C4EEF82318477EA74F0A482CEC0BF43136936_AdjustorThunk','_RemapManagedArraysJob_CleanupJobFn_Gen_m01C375C94218A2CFE139EBAB60EB371DFDD72184_AdjustorThunk','_GatherChunks_Execute_m93D984555F5A67D6304412EB723597C8872CBC1C_AdjustorThunk','_GatherChunks_PrepareJobAtExecuteTimeFn_Gen_m01455E77C09A899C88190705624E57F6C169F99C_AdjustorThunk','_GatherChunks_CleanupJobFn_Gen_mF4EF0E7D4488DF7101F47A535D41CC5B2D5E1606_AdjustorThunk','_GatherChunksWithFiltering_Execute_mD26E36056038569B432F3C57C00E898346E6A863_AdjustorThunk','_GatherChunksWithFiltering_PrepareJobAtExecuteTimeFn_Gen_mC42992D3E1B183160324236233DABD9521A1EF66_AdjustorThunk','_GatherChunksWithFiltering_CleanupJobFn_Gen_m91C6E595B0D33283980991F28993EE1F739CF3F0_AdjustorThunk','_JoinChunksJob_Execute_m02E9EDAFF4FB39EC656D7766889F0C5FFB47C6BC_AdjustorThunk','_JoinChunksJob_PrepareJobAtExecuteTimeFn_Gen_mF153D83B354AB4A4CA3743FDEABF2C72D7224B61_AdjustorThunk','_JoinChunksJob_CleanupJobFn_Gen_m12A422D06E1E72C835CDC2EE6365F2E0B5D9E6BD_AdjustorThunk','_GC_default_warn_proc','__ZN4bgfx2gl17RendererContextGL18destroyIndexBufferENS_17IndexBufferHandleE','__ZN4bgfx2gl17RendererContextGL19destroyVertexLayoutENS_18VertexLayoutHandleE','__ZN4bgfx2gl17RendererContextGL19destroyVertexBufferENS_18VertexBufferHandleE','__ZN4bgfx2gl17RendererContextGL25destroyDynamicIndexBufferENS_17IndexBufferHandleE','__ZN4bgfx2gl17RendererContextGL26destroyDynamicVertexBufferENS_18VertexBufferHandleE','__ZN4bgfx2gl17RendererContextGL13destroyShaderENS_12ShaderHandleE','__ZN4bgfx2gl17RendererContextGL14destroyProgramENS_13ProgramHandleE','__ZN4bgfx2gl17RendererContextGL14destroyTextureENS_13TextureHandleE','__ZN4bgfx2gl17RendererContextGL18destroyFrameBufferENS_17FrameBufferHandleE','__ZN4bgfx2gl17RendererContextGL14destroyUniformENS_13UniformHandleE','__ZN4bgfx2gl17RendererContextGL24invalidateOcclusionQueryENS_20OcclusionQueryHandleE','__ZN4bgfx2gl17RendererContextGL9blitSetupERNS_19TextVideoMemBlitterE','__ZN2bx6packA8EPvPKf','__ZN2bx8unpackA8EPfPKv','__ZN2bx6packR8EPvPKf','__ZN2bx8unpackR8EPfPKv','__ZN2bx7packR8IEPvPKf','__ZN2bx9unpackR8IEPfPKv','__ZN2bx7packR8UEPvPKf','__ZN2bx9unpackR8UEPfPKv','__ZN2bx7packR8SEPvPKf','__ZN2bx9unpackR8SEPfPKv','__ZN2bx7packR16EPvPKf','__ZN2bx9unpackR16EPfPKv','__ZN2bx8packR16IEPvPKf','__ZN2bx10unpackR16IEPfPKv','__ZN2bx8packR16UEPvPKf','__ZN2bx10unpackR16UEPfPKv','__ZN2bx8packR16FEPvPKf','__ZN2bx10unpackR16FEPfPKv','__ZN2bx8packR16SEPvPKf','__ZN2bx10unpackR16SEPfPKv','__ZN2bx8packR32IEPvPKf','__ZN2bx10unpackR32IEPfPKv','__ZN2bx8packR32UEPvPKf','__ZN2bx10unpackR32UEPfPKv','__ZN2bx8packR32FEPvPKf','__ZN2bx10unpackR32FEPfPKv','__ZN2bx7packRg8EPvPKf','__ZN2bx9unpackRg8EPfPKv','__ZN2bx8packRg8IEPvPKf','__ZN2bx10unpackRg8IEPfPKv','__ZN2bx8packRg8UEPvPKf','__ZN2bx10unpackRg8UEPfPKv','__ZN2bx8packRg8SEPvPKf','__ZN2bx10unpackRg8SEPfPKv','__ZN2bx8packRg16EPvPKf','__ZN2bx10unpackRg16EPfPKv','__ZN2bx9packRg16IEPvPKf','__ZN2bx11unpackRg16IEPfPKv','__ZN2bx9packRg16UEPvPKf','__ZN2bx11unpackRg16UEPfPKv','__ZN2bx9packRg16FEPvPKf','__ZN2bx11unpackRg16FEPfPKv','__ZN2bx9packRg16SEPvPKf','__ZN2bx11unpackRg16SEPfPKv','__ZN2bx9packRg32IEPvPKf','__ZN2bx11unpackRg32IEPfPKv','__ZN2bx9packRg32UEPvPKf','__ZN2bx11unpackRg32UEPfPKv','__ZN2bx9packRg32FEPvPKf','__ZN2bx11unpackRg32FEPfPKv','__ZN2bx8packRgb8EPvPKf','__ZN2bx10unpackRgb8EPfPKv','__ZN2bx9packRgb8SEPvPKf','__ZN2bx11unpackRgb8SEPfPKv','__ZN2bx9packRgb8IEPvPKf','__ZN2bx11unpackRgb8IEPfPKv','__ZN2bx9packRgb8UEPvPKf','__ZN2bx11unpackRgb8UEPfPKv','__ZN2bx11packRgb9E5FEPvPKf','__ZN2bx13unpackRgb9E5FEPfPKv','__ZN2bx9packBgra8EPvPKf','__ZN2bx11unpackBgra8EPfPKv','__ZN2bx9packRgba8EPvPKf','__ZN2bx11unpackRgba8EPfPKv','__ZN2bx10packRgba8IEPvPKf','__ZN2bx12unpackRgba8IEPfPKv','__ZN2bx10packRgba8UEPvPKf','__ZN2bx12unpackRgba8UEPfPKv','__ZN2bx10packRgba8SEPvPKf','__ZN2bx12unpackRgba8SEPfPKv','__ZN2bx10packRgba16EPvPKf','__ZN2bx12unpackRgba16EPfPKv','__ZN2bx11packRgba16IEPvPKf','__ZN2bx13unpackRgba16IEPfPKv','__ZN2bx11packRgba16UEPvPKf','__ZN2bx13unpackRgba16UEPfPKv','__ZN2bx11packRgba16FEPvPKf','__ZN2bx13unpackRgba16FEPfPKv','__ZN2bx11packRgba16SEPvPKf','__ZN2bx13unpackRgba16SEPfPKv','__ZN2bx11packRgba32IEPvPKf','__ZN2bx13unpackRgba32IEPfPKv','__ZN2bx11packRgba32UEPvPKf','__ZN2bx13unpackRgba32UEPfPKv','__ZN2bx11packRgba32FEPvPKf','__ZN2bx13unpackRgba32FEPfPKv','__ZN2bx10packR5G6B5EPvPKf','__ZN2bx12unpackR5G6B5EPfPKv','__ZN2bx9packRgba4EPvPKf','__ZN2bx11unpackRgba4EPfPKv','__ZN2bx10packRgb5a1EPvPKf','__ZN2bx12unpackRgb5a1EPfPKv','__ZN2bx11packRgb10A2EPvPKf','__ZN2bx13unpackRgb10A2EPfPKv','__ZN2bx12packRG11B10FEPvPKf','__ZN2bx14unpackRG11B10FEPfPKv','__ZN2bx7packR24EPvPKf','__ZN2bx9unpackR24EPfPKv','__ZN2bx9packR24G8EPvPKf','__ZN2bx11unpackR24G8EPfPKv','__ZN4bgfx4noop19RendererContextNOOP18destroyIndexBufferENS_17IndexBufferHandleE','__ZN4bgfx4noop19RendererContextNOOP19destroyVertexLayoutENS_18VertexLayoutHandleE','__ZN4bgfx4noop19RendererContextNOOP19destroyVertexBufferENS_18VertexBufferHandleE','__ZN4bgfx4noop19RendererContextNOOP25destroyDynamicIndexBufferENS_17IndexBufferHandleE','__ZN4bgfx4noop19RendererContextNOOP26destroyDynamicVertexBufferENS_18VertexBufferHandleE','__ZN4bgfx4noop19RendererContextNOOP13destroyShaderENS_12ShaderHandleE','__ZN4bgfx4noop19RendererContextNOOP14destroyProgramENS_13ProgramHandleE','__ZN4bgfx4noop19RendererContextNOOP14destroyTextureENS_13TextureHandleE','__ZN4bgfx4noop19RendererContextNOOP18destroyFrameBufferENS_17FrameBufferHandleE','__ZN4bgfx4noop19RendererContextNOOP14destroyUniformENS_13UniformHandleE','__ZN4bgfx4noop19RendererContextNOOP24invalidateOcclusionQueryENS_20OcclusionQueryHandleE','__ZN4bgfx4noop19RendererContextNOOP9blitSetupERNS_19TextVideoMemBlitterE','_JobStruct_1_ProducerExecuteFn_Gen_m95CBD8D957F15017013E904D8BE1A19079BEDBF6','_ReversePInvokeWrapper_JobStruct_1_ProducerExecuteFn_Gen_m95CBD8D957F15017013E904D8BE1A19079BEDBF6','_ParallelForJobStruct_1_ProducerExecuteFn_Gen_m5409D32EF29144F8E51FF8B2CAD6094C3A9056C8','_ReversePInvokeWrapper_ParallelForJobStruct_1_ProducerExecuteFn_Gen_m5409D32EF29144F8E51FF8B2CAD6094C3A9056C8','_JobChunk_Process_1_ProducerExecuteFn_Gen_m20D81F45903C3CB82D578B893CE56DD2CF3A8B8E','_ReversePInvokeWrapper_JobChunk_Process_1_ProducerExecuteFn_Gen_m20D81F45903C3CB82D578B893CE56DD2CF3A8B8E','_JobChunk_Process_1_ProducerExecuteFn_Gen_m82DF6FCF7223A0FD20318A776B1B9E5C28D6B471','_ReversePInvokeWrapper_JobChunk_Process_1_ProducerExecuteFn_Gen_m82DF6FCF7223A0FD20318A776B1B9E5C28D6B471','_CarAudioSystem_U3COnUpdateU3Eb__0_0_mFB69A0075371C0CAAB141894C9F4920FED505AD2','_CarAudioSystem_U3COnUpdateU3Eb__0_2_mE42A5B66C31335991A29D4CB4D46238772F07672','_JobChunk_Process_1_ProducerExecuteFn_Gen_mC1EA5FDA41E4C007B435609CB82464DBEE26F023','_ReversePInvokeWrapper_JobChunk_Process_1_ProducerExecuteFn_Gen_mC1EA5FDA41E4C007B435609CB82464DBEE26F023','_JobChunk_Process_1_ProducerExecuteFn_Gen_mD8D9E7CC08CE4805303596C7824057C78A0722DC','_ReversePInvokeWrapper_JobChunk_Process_1_ProducerExecuteFn_Gen_mD8D9E7CC08CE4805303596C7824057C78A0722DC','_JobChunk_Process_1_ProducerExecuteFn_Gen_m8673E8AF984974BC250E7F9597DBA83B1EE5D78E','_ReversePInvokeWrapper_JobChunk_Process_1_ProducerExecuteFn_Gen_m8673E8AF984974BC250E7F9597DBA83B1EE5D78E','_U3CU3Ec__DisplayClass5_0_U3COnUpdateU3Eb__0_m75B9983438829AB16C524E9A6775875E6A2F6DC6','_U3CU3Ec_U3COnUpdateU3Eb__5_2_m531AAF4CC48E3F24402760144F30335B4210A243','_JobChunk_Process_1_ProducerExecuteFn_Gen_m561D56464C3CA9AEA8BEC6A7C964982C9AC213BF','_ReversePInvokeWrapper_JobChunk_Process_1_ProducerExecuteFn_Gen_m561D56464C3CA9AEA8BEC6A7C964982C9AC213BF','_JobChunk_Process_1_ProducerExecuteFn_Gen_mE2633F64F8A6C03CE09A3F8A6960F613FFA85373','_ReversePInvokeWrapper_JobChunk_Process_1_ProducerExecuteFn_Gen_mE2633F64F8A6C03CE09A3F8A6960F613FFA85373','_JobChunk_Process_1_ProducerExecuteFn_Gen_mAD46F96B8FD933299A5D1005925AC64C17A4B9E0','_ReversePInvokeWrapper_JobChunk_Process_1_ProducerExecuteFn_Gen_mAD46F96B8FD933299A5D1005925AC64C17A4B9E0','_U3CU3Ec__DisplayClass0_0_U3COnUpdateU3Eb__0_m949AE7ABC6A8D9433C9B8F5832DE0FBBEB01A103','_U3CU3Ec__DisplayClass1_0_U3COnUpdateU3Eb__1_mE2EB8CFB82D2D331239F72E8BA7A841EAC28DD23','_UpdateGameOverMenu_U3CSetMenuVisibilityU3Eb__2_0_m2DAD37E78B92ACB6B10CB44A28E1819589528330','_UpdateGameOverMenu_U3CSetMenuVisibilityU3Eb__2_1_m5664EA053E81D91F8B945910295CA6FA0AA35A99','_UpdateGameOverMenu_U3CSetMenuVisibilityU3Eb__2_2_m2EE13033885A09E1451C82F6A2ADC2953811F16A','_U3CU3Ec__DisplayClass1_0_U3COnUpdateU3Eb__3_mD0767017CE62795DC4639378F4E386BF7F02A06C','_U3CU3Ec__DisplayClass1_0_U3COnUpdateU3Eb__4_m0AA1DA1FF6B5B979D14CFECBA50A6A6CA3D8D01B','_U3CU3Ec__DisplayClass1_0_U3COnUpdateU3Eb__5_mF4DF40B6149E1B1EC774D706657B17D5DCC51E18','_U3CU3Ec__DisplayClass1_0_U3COnUpdateU3Eb__6_m8919A75A65A2BCE1B5BE97415274B949A4E3D9FE','_U3CU3Ec__DisplayClass1_0_U3COnUpdateU3Eb__7_mE5AB092B246F2CBAE1FE32D6DFA3B6F697E386E9','_UpdateGameplayMenu_U3CSetMenuVisibilityU3Eb__2_0_m1E4513FDE3230F6B0159A71533D107C779E77FBE','_UpdateGameplayMenu_U3CSetMenuVisibilityU3Eb__2_1_m480617EE1C341E7C9D71DBBEDF32922DC16D1B3D','_UpdateGameplayMenu_U3CSetMenuVisibilityU3Eb__2_2_mF9FFBC52E7115BA297F1FCB71F5D2EC2E0DC64C6','_UpdateGameplayMenu_U3CSetMenuVisibilityU3Eb__2_3_mB8DBA73571BAEA4D7AC66864BFC8353753289ED5','_UpdateGameplayMenu_U3CSetMenuVisibilityU3Eb__2_4_m1C2019F7A9411962D8B024C74B887CA3FD53A5E4','_UpdateMainMenu_U3CSetMenuVisibilityU3Eb__2_0_mE2A867DFA8EEFDAEE9343225605DA12693319B89','_UpdateMainMenu_U3CSetMenuVisibilityU3Eb__2_1_mF739DC1D8D65278ED160A8A48908ED7E79AE9E3A','_UpdateMainMenu_U3CSetMenuVisibilityU3Eb__2_2_m47F5213811689A81390DDC71CD9ACD0FFFEFEAB1','_JobChunk_Process_1_ProducerExecuteFn_Gen_mDEC238D59778BAE34232FA59918E9694AFD1FCC4','_ReversePInvokeWrapper_JobChunk_Process_1_ProducerExecuteFn_Gen_mDEC238D59778BAE34232FA59918E9694AFD1FCC4','_JobChunk_Process_1_ProducerExecuteFn_Gen_m81FC6283E382FFEC19E8C94A6A3C75733A3D1CF4','_ReversePInvokeWrapper_JobChunk_Process_1_ProducerExecuteFn_Gen_m81FC6283E382FFEC19E8C94A6A3C75733A3D1CF4','_U3CU3Ec__DisplayClass5_1_U3COnUpdateU3Eb__4_m14A88E2CFD9B02C824A4B6D7647D4CD2807FC937','_U3CU3Ec__DisplayClass5_0_U3COnUpdateU3Eb__5_mEEB709AB5BA673E478A67D43556814EE6B18F27B','_U3CU3Ec_U3COnUpdateU3Eb__1_0_m9478441D6C1037A723A1A0EB6001CE4B38025BE5','_U3CU3Ec_U3COnUpdateU3Eb__1_1_mAD7781674849ED6B84EAE1D3AF30F6E25E3B6572','_U3CU3Ec__DisplayClass1_0_U3COnUpdateU3Eb__2_m18EEE8252E3429D6774CB1E88DEEA4D39993DC09','_U3CU3Ec__DisplayClass95_0_U3CReloadAllImagesU3Eb__0_m34E04B286B17711E8ABB8DBDCD4CCED40673F1BE','_U3CU3Ec__DisplayClass96_0_U3CDestroyAllTexturesU3Eb__1_m92D29E97B028AD3EFCCE3D1E9B66BE57458227DC','_U3CU3Ec__DisplayClass96_0_U3CDestroyAllTexturesU3Eb__2_m8DDD63AF4B074F49BE9AE6700BE73A791FD61363','_U3CU3Ec__DisplayClass97_0_U3CShutdownU3Eb__1_mD37FDEDA74960AE455722E4E803BBAAB5EB55447','_U3CU3Ec__DisplayClass97_0_U3CShutdownU3Eb__2_mD7B477F32BBD4C81E8A87FEE38037E8B950541CC','_JobChunk_Process_1_ProducerExecuteFn_Gen_mF24A6DF29FA90E2F10AB546B1BED0FD93E50D95E','_ReversePInvokeWrapper_JobChunk_Process_1_ProducerExecuteFn_Gen_mF24A6DF29FA90E2F10AB546B1BED0FD93E50D95E','_U3CU3Ec__DisplayClass1_0_U3COnUpdateU3Eb__0_mE433E5CA0D11A03A97969C43D129B0AC13204991','_U3CU3Ec__DisplayClass5_0_U3COnUpdateU3Eb__0_mBB945BF042B0B8142B48C98784E96B6CECDDF05A','_U3CU3Ec__DisplayClass5_0_U3COnUpdateU3Eb__1_m07BCF222F9E4856B4B5760A718B36F6CC4360C74','_U3CU3Ec__DisplayClass5_0_U3COnUpdateU3Eb__4_mA2EA38AB4C7F86D805C722F57681CE2756094290','_U3CU3Ec__DisplayClass5_0_U3COnUpdateU3Eb__5_mE11458B12BBAABA0C51C014D5D35E6245FD7812C','_U3CU3Ec__DisplayClass5_0_U3COnUpdateU3Eb__6_mD1ED596DEA3F60ADA9FCE9CA35333A0310FA9ED5','_JobStruct_1_ProducerExecuteFn_Gen_m9A800A08900F3AE89FD6CCA733478857FFE392DE','_ReversePInvokeWrapper_JobStruct_1_ProducerExecuteFn_Gen_m9A800A08900F3AE89FD6CCA733478857FFE392DE','_JobStruct_1_ProducerExecuteFn_Gen_mC68BC278F6AD2B36EFBBB3B85F23289B65FC4928','_ReversePInvokeWrapper_JobStruct_1_ProducerExecuteFn_Gen_mC68BC278F6AD2B36EFBBB3B85F23289B65FC4928','_JobStruct_1_ProducerExecuteFn_Gen_m9F3DF1243D230ADF0B4DBA21F152A7B69E5B7A01','_ReversePInvokeWrapper_JobStruct_1_ProducerExecuteFn_Gen_m9F3DF1243D230ADF0B4DBA21F152A7B69E5B7A01','_ParallelForJobStruct_1_ProducerExecuteFn_Gen_m14BBE3F7B169ADF49FB879EDB807D74680DCAC12','_ReversePInvokeWrapper_ParallelForJobStruct_1_ProducerExecuteFn_Gen_m14BBE3F7B169ADF49FB879EDB807D74680DCAC12','_JobStruct_1_ProducerExecuteFn_Gen_m031EFEE1AA99761320856AC863CAC606B3FA36B0','_ReversePInvokeWrapper_JobStruct_1_ProducerExecuteFn_Gen_m031EFEE1AA99761320856AC863CAC606B3FA36B0','_JobStruct_1_ProducerExecuteFn_Gen_m74BEC5DA15A5B560F54BA09783EE1245A9A0A4A9','_ReversePInvokeWrapper_JobStruct_1_ProducerExecuteFn_Gen_m74BEC5DA15A5B560F54BA09783EE1245A9A0A4A9','_JobStruct_1_ProducerExecuteFn_Gen_mF094104BF9A2304D902D7E00A025CAB8FE50E3E7','_ReversePInvokeWrapper_JobStruct_1_ProducerExecuteFn_Gen_mF094104BF9A2304D902D7E00A025CAB8FE50E3E7','_JobStruct_1_ProducerExecuteFn_Gen_m6191945CCA4FD37B31C856F28060C985598603A0','_ReversePInvokeWrapper_JobStruct_1_ProducerExecuteFn_Gen_m6191945CCA4FD37B31C856F28060C985598603A0','_ParallelForJobStruct_1_ProducerExecuteFn_Gen_m1A750F7F52F392BF54A0915E81F1C56C31CF0F0D','_ReversePInvokeWrapper_ParallelForJobStruct_1_ProducerExecuteFn_Gen_m1A750F7F52F392BF54A0915E81F1C56C31CF0F0D','_ParallelForJobStruct_1_ProducerExecuteFn_Gen_m89ED1F45B9A332EE3A4A4CB650017F7BAB07B9B9','_ReversePInvokeWrapper_ParallelForJobStruct_1_ProducerExecuteFn_Gen_m89ED1F45B9A332EE3A4A4CB650017F7BAB07B9B9','_ParallelForJobStruct_1_ProducerExecuteFn_Gen_m45C22DCCB4AEFB0D5BCBAC5D489CA05135114220','_ReversePInvokeWrapper_ParallelForJobStruct_1_ProducerExecuteFn_Gen_m45C22DCCB4AEFB0D5BCBAC5D489CA05135114220','_ParallelForJobStruct_1_ProducerExecuteFn_Gen_mE41E44B3BA09BAF3B7A5D1D1D255DD3AF28277AE','_ReversePInvokeWrapper_ParallelForJobStruct_1_ProducerExecuteFn_Gen_mE41E44B3BA09BAF3B7A5D1D1D255DD3AF28277AE','_ParallelForJobStruct_1_ProducerExecuteFn_Gen_m1EF9FBF2DFC1E025CE18A11618D2B2AC0D750997','_ReversePInvokeWrapper_ParallelForJobStruct_1_ProducerExecuteFn_Gen_m1EF9FBF2DFC1E025CE18A11618D2B2AC0D750997','_JobStruct_1_ProducerExecuteFn_Gen_m6C9B14E42F6A11421FD115496A381CA53052382F','_ReversePInvokeWrapper_JobStruct_1_ProducerExecuteFn_Gen_m6C9B14E42F6A11421FD115496A381CA53052382F','_JobStruct_1_ProducerExecuteFn_Gen_mE782C890B78BDB3A29D1B1CC7CEF562FF777058F','_ReversePInvokeWrapper_JobStruct_1_ProducerExecuteFn_Gen_mE782C890B78BDB3A29D1B1CC7CEF562FF777058F','_ParallelForJobStruct_1_ProducerExecuteFn_Gen_mB33A3B8F893FC4D225D68B58A4C4CC9B54DB1F07','_ReversePInvokeWrapper_ParallelForJobStruct_1_ProducerExecuteFn_Gen_mB33A3B8F893FC4D225D68B58A4C4CC9B54DB1F07','_JobChunk_Process_1_ProducerExecuteFn_Gen_mC19217D340D13A25D2DBFBCE9C1687723A303EB5','_ReversePInvokeWrapper_JobChunk_Process_1_ProducerExecuteFn_Gen_mC19217D340D13A25D2DBFBCE9C1687723A303EB5','_ParallelForJobStruct_1_ProducerExecuteFn_Gen_m0A312D00285BCEF66450D70CA652BA8321BAEA5F','_ReversePInvokeWrapper_ParallelForJobStruct_1_ProducerExecuteFn_Gen_m0A312D00285BCEF66450D70CA652BA8321BAEA5F','_JobChunk_Process_1_ProducerExecuteFn_Gen_mA53B53A85AC4346B8CEFE2823FBDA4C9DB78044F','_ReversePInvokeWrapper_JobChunk_Process_1_ProducerExecuteFn_Gen_mA53B53A85AC4346B8CEFE2823FBDA4C9DB78044F','_JobChunk_Process_1_ProducerExecuteFn_Gen_m57CB65231DF8994DE71EB6934BEFB36186DC954D','_ReversePInvokeWrapper_JobChunk_Process_1_ProducerExecuteFn_Gen_m57CB65231DF8994DE71EB6934BEFB36186DC954D','_JobChunk_Process_1_ProducerExecuteFn_Gen_mE9B9B4E7BB06318FE716A529DBAEA68F866AE740','_ReversePInvokeWrapper_JobChunk_Process_1_ProducerExecuteFn_Gen_mE9B9B4E7BB06318FE716A529DBAEA68F866AE740','_JobChunk_Process_1_ProducerExecuteFn_Gen_mD3EE34ABEA095B29A04A1221AB32E0FC0DFE7186','_ReversePInvokeWrapper_JobChunk_Process_1_ProducerExecuteFn_Gen_mD3EE34ABEA095B29A04A1221AB32E0FC0DFE7186','_JobStruct_1_ProducerExecuteFn_Gen_m05F2B6491AA85B78DF8D68B424FCEE6AB25A939A','_ReversePInvokeWrapper_JobStruct_1_ProducerExecuteFn_Gen_m05F2B6491AA85B78DF8D68B424FCEE6AB25A939A','_JobStruct_1_ProducerExecuteFn_Gen_m6CB571240CCB4C02C8CBF1FE9D707969946CC95F','_ReversePInvokeWrapper_JobStruct_1_ProducerExecuteFn_Gen_m6CB571240CCB4C02C8CBF1FE9D707969946CC95F','_JobChunk_Process_1_ProducerExecuteFn_Gen_m55001EA32943F355019558C71283AF9A29A4C357','_ReversePInvokeWrapper_JobChunk_Process_1_ProducerExecuteFn_Gen_m55001EA32943F355019558C71283AF9A29A4C357','_JobStruct_1_ProducerExecuteFn_Gen_mC121D74DCAA72DCBBA5D7E756FB4BCE30D4B625A','_ReversePInvokeWrapper_JobStruct_1_ProducerExecuteFn_Gen_mC121D74DCAA72DCBBA5D7E756FB4BCE30D4B625A','_JobChunk_Process_1_ProducerExecuteFn_Gen_m2EB96584C50B8EB4ED1FDD4D8D9732F944AE8272','_ReversePInvokeWrapper_JobChunk_Process_1_ProducerExecuteFn_Gen_m2EB96584C50B8EB4ED1FDD4D8D9732F944AE8272','_JobChunk_Process_1_ProducerExecuteFn_Gen_m695C0E98BF219ED7D80FBF261CBB74C04B2A6137','_ReversePInvokeWrapper_JobChunk_Process_1_ProducerExecuteFn_Gen_m695C0E98BF219ED7D80FBF261CBB74C04B2A6137','_JobChunk_Process_1_ProducerExecuteFn_Gen_mFC516F47DE9388EC152F60A7A6F4DC573DA7D912','_ReversePInvokeWrapper_JobChunk_Process_1_ProducerExecuteFn_Gen_mFC516F47DE9388EC152F60A7A6F4DC573DA7D912','_JobChunk_Process_1_ProducerExecuteFn_Gen_mC3B8A2E5E332EAA88B5737AD0FDBE182C4369AEE','_ReversePInvokeWrapper_JobChunk_Process_1_ProducerExecuteFn_Gen_mC3B8A2E5E332EAA88B5737AD0FDBE182C4369AEE','_JobChunk_Process_1_ProducerExecuteFn_Gen_m9A25B066FCE97D46108EA6E784AEAF1CE6EC1798','_ReversePInvokeWrapper_JobChunk_Process_1_ProducerExecuteFn_Gen_m9A25B066FCE97D46108EA6E784AEAF1CE6EC1798','_U3CU3Ec__DisplayClass2_0_U3COnUpdateU3Eb__0_m91062E044ED0E6966C9DE2EF173BA0904BDEF5DE','_U3CU3Ec__DisplayClass2_0_U3COnUpdateU3Eb__1_mB408CC63D9C37D30E5A53EA6677A38E5CC853450','_UpdateLightMatricesSystem_U3COnUpdateU3Eb__0_0_m2E333E0AF243F78EBB124B1581B092DEDFD0C7B9','_UpdateLightMatricesSystem_U3COnUpdateU3Eb__0_1_m6D7A2B75C69EBD63B8655429FDB913D0F1945945','_U3CU3Ec__DisplayClass0_0_U3COnUpdateU3Eb__0_m0E8BC2527CC3597126CEB818E8A1FE98B8D9CFBA','_JobChunk_Process_1_ProducerExecuteFn_Gen_mA61082BEA79B8F5AE866974BBB1764FF257751EF','_ReversePInvokeWrapper_JobChunk_Process_1_ProducerExecuteFn_Gen_mA61082BEA79B8F5AE866974BBB1764FF257751EF','_U3CU3Ec__DisplayClass7_0_U3COnUpdateU3Eb__0_m69465EA8081E657462A5E571D4B1026C1193F346','_GC_ignore_warn_proc','__ZN4bgfx2glL15stubPolygonModeEjj','__ZN4bgfx2glL23stubVertexAttribDivisorEjj','__ZN4bgfx2glL21stubInsertEventMarkerEiPKc','_emscripten_glVertexAttribDivisorANGLE','_emscripten_glAttachShader','_emscripten_glBindBuffer','_emscripten_glBindFramebuffer','_emscripten_glBindRenderbuffer','_emscripten_glBindTexture','_emscripten_glBlendEquationSeparate','_emscripten_glBlendFunc','_emscripten_glDeleteBuffers','_emscripten_glDeleteFramebuffers','_emscripten_glDeleteRenderbuffers','_emscripten_glDeleteTextures','_emscripten_glDetachShader','_emscripten_glGenBuffers','_emscripten_glGenFramebuffers','_emscripten_glGenRenderbuffers','_emscripten_glGenTextures','_emscripten_glGetBooleanv','_emscripten_glGetFloatv','_emscripten_glGetIntegerv','_emscripten_glHint','_emscripten_glPixelStorei','_emscripten_glStencilMaskSeparate','_emscripten_glUniform1i','_emscripten_glVertexAttrib1fv','_emscripten_glVertexAttrib2fv','_emscripten_glVertexAttrib3fv','_emscripten_glVertexAttrib4fv','_emscripten_glGenQueriesEXT','_emscripten_glDeleteQueriesEXT','_emscripten_glBeginQueryEXT','_emscripten_glQueryCounterEXT','_emscripten_glDeleteVertexArraysOES','_emscripten_glGenVertexArraysOES','_emscripten_glDrawBuffersWEBGL','_emscripten_glGenQueries','_emscripten_glDeleteQueries','_emscripten_glBeginQuery','_emscripten_glDrawBuffers','_emscripten_glDeleteVertexArrays','_emscripten_glGenVertexArrays','_emscripten_glVertexAttribI4iv','_emscripten_glVertexAttribI4uiv','_emscripten_glUniform1ui','_emscripten_glGetInteger64v','_emscripten_glGenSamplers','_emscripten_glDeleteSamplers','_emscripten_glBindSampler','_emscripten_glVertexAttribDivisor','_emscripten_glBindTransformFeedback','_emscripten_glDeleteTransformFeedbacks','_emscripten_glGenTransformFeedbacks','_emscripten_glVertexAttribDivisorNV','_emscripten_glVertexAttribDivisorEXT','_emscripten_glVertexAttribDivisorARB','_emscripten_glDrawBuffersEXT'];
var debug_table_viif = [0,'_emscripten_glTexParameterf$legalf32','_emscripten_glSamplerParameterf$legalf32',0];
var debug_table_viifi = [0,'_emscripten_glClearBufferfi$legalf32'];
var debug_table_viii = [0,'_ManagedJobForEachDelegate_Invoke_m3AC993F0DAE9EE461BB43E8EBC03138ACCDE003F','_MemoryBinaryReader_ReadBytes_mC92A1A4EE6BB0D6AB0A68D554B53DF00DC8B8E24','_RetainBlobAssetSystem_OnUpdate_m66C5C4CAC1C15CA6A1648783B9375708F8C8E6EE','_ParentSystem_OnUpdate_mC874FA62BE1C461FB438738F5308C74235376EAE','_CompositeScaleSystem_OnUpdate_m8FB9DE0C4A803A39C8AE77FA46E6B466416FD595','_RotationEulerSystem_OnUpdate_m54010EF7BBD4CFA84987BEE0E975D2ECB1BCE782','_PostRotationEulerSystem_OnUpdate_mCA581312AA1EEAD981D0C3EB922D277561327409','_CompositeRotationSystem_OnUpdate_mAC4CAFA475A98011E2EF6848E295155DBBC67502','_TRSToLocalToWorldSystem_OnUpdate_m1BAF0945BD61477B3E4D7F050DD3B6E030C58EA5','_ParentScaleInverseSystem_OnUpdate_m111C043E44C3E150F19BF804991B69E75867FD60','_TRSToLocalToParentSystem_OnUpdate_m2B27D511140B53487172F3ECEC4D0D3A46627FD5','_LocalToParentSystem_OnUpdate_m2EA7CE654C3CB07B51748F8440210CA5E2D5F025','_WorldToLocalSystem_OnUpdate_m08B65F0DFE8351DBDD7EFADB4AB2F27E6DF16604','_SubmitSimpleLitMeshChunked_OnUpdate_mF99A9310604E0728F3A29C7509487D63B1543428','_BumpCar_OnUpdate_mCBFC96C45498DCD34FA0A0FFD05C6B3F6988CA88','_CollideWalls_OnUpdate_m18399E6A71E31A58A5ABB155C96B7A6F35FDD116','_InitializeCarResetState_OnUpdate_mDF7BDB41E00F660013D4282C02CA72646AF386EA','_MoveCar_OnUpdate_m75CF42D13F9BDFBDE0ABD2FBCAD05D6C4BB7C423','_Rotate_OnUpdate_m7ADC0E4DC0E8FC6672E66645948938A9D3439B3A','_RotateCar_OnUpdate_mC28E3455DA6FE9432671F9A2A1B1A314A5934037','_UpdateCarAIInputs_OnUpdate_mCF07E68C2155872EB6334F70B8D952CE4126427D','_UpdateCarLapProgress_OnUpdate_mD9BAAF34160B199BFAD872563AD80C4BAB95A247','_UpdateRaceTimer_OnUpdate_m99AD7C4B6D38E3FC9D1FAE2F108D7C970BC16C8A','_UpdateSmoke_OnUpdate_mDB50839E8DA84B0F76F866AD0CF2AFC4D7460FB6','_Action_2_Invoke_m25F6327A8B1EB2C9D5BB8B8988B156872D528584','_F_ED_1_Invoke_m5DFFE9540F25FEE0258F8169FDF0953490512C17','__ZN4bgfx2gl17RendererContextGL18createVertexLayoutENS_18VertexLayoutHandleERKNS_12VertexLayoutE','__ZN4bgfx2gl17RendererContextGL12createShaderENS_12ShaderHandleEPKNS_6MemoryE','__ZN4bgfx2gl17RendererContextGL16overrideInternalENS_13TextureHandleEm','__ZN4bgfx2gl17RendererContextGL17requestScreenShotENS_17FrameBufferHandleEPKc','__ZN4bgfx2gl17RendererContextGL14updateViewNameEtPKc','__ZN4bgfx2gl17RendererContextGL9setMarkerEPKct','__ZN4bgfx2gl17RendererContextGL10blitRenderERNS_19TextVideoMemBlitterEj','__ZN4bgfx4noop19RendererContextNOOP18createVertexLayoutENS_18VertexLayoutHandleERKNS_12VertexLayoutE','__ZN4bgfx4noop19RendererContextNOOP12createShaderENS_12ShaderHandleEPKNS_6MemoryE','__ZN4bgfx4noop19RendererContextNOOP16overrideInternalENS_13TextureHandleEm','__ZN4bgfx4noop19RendererContextNOOP17requestScreenShotENS_17FrameBufferHandleEPKc','__ZN4bgfx4noop19RendererContextNOOP14updateViewNameEtPKc','__ZN4bgfx4noop19RendererContextNOOP9setMarkerEPKct','__ZN4bgfx4noop19RendererContextNOOP10blitRenderERNS_19TextVideoMemBlitterEj','__ZN4bgfx12CallbackStub12captureFrameEPKvj','__ZN4bgfx11CallbackC9912captureFrameEPKvj','_U3CU3Ec__DisplayClass2_0_U3COnUpdateU3Eb__0_m69EAD182E8004947E102748C3D28AE58C16CCED6','_U3CU3Ec__DisplayClass2_0_U3COnUpdateU3Eb__1_mDC5F7FFE0082152CB9BCCE44E8F6AB39CD3F86AF','_U3CU3Ec__DisplayClass2_0_U3COnUpdateU3Eb__0_m31573A54875A2C59E1DB5771F50EF1E53070386A','_U3CU3Ec__DisplayClass2_0_U3COnUpdateU3Eb__1_m2621A5D98AAAA502994A494A5B7F3ABC35AA9879','_CarAudioSystem_U3COnUpdateU3Eb__0_1_mABDDCDE9A75D1E5CCF8914AE14B07C3E0596F9BF','_U3CU3Ec__DisplayClass0_0_U3COnUpdateU3Eb__3_mDF9D34EE06951AE1E5EC5BCD084B01EEAFF1D9ED','_U3CU3Ec__DisplayClass1_0_U3COnUpdateU3Eb__0_mFDE94DEC5FA8E02ABB8A522F78455C8BFCC2E487','_U3CU3Ec__DisplayClass1_0_U3COnUpdateU3Eb__0_m51821E665506FDD249D33A3EC2162D84B8AB16E6','_U3CU3Ec__DisplayClass1_0_U3COnUpdateU3Eb__0_m8C22C338C15AA4F130672BE19BC07DC2B3968B5D','_U3CU3Ec__DisplayClass1_0_U3COnUpdateU3Eb__1_mB2D3FE214B507CA955CD0BCA9DBDF4BA2458BBFC','_U3CU3Ec__DisplayClass1_0_U3COnUpdateU3Eb__2_m90E2467BA0CAE2E6CBD42B7C556B22F9BF6571F3','_U3CU3Ec__DisplayClass5_0_U3COnUpdateU3Eb__1_m6AFBBA09FF87995493C532C34E7D4762F940BB67','_U3CU3Ec__DisplayClass5_0_U3COnUpdateU3Eb__2_m8E1BEE4AEF9F95AD709F3B17EA1EAFF5ABD16BC6','_U3CU3Ec__DisplayClass5_0_U3COnUpdateU3Eb__3_mBF38E2B541EE38275DF76CC02BFC53D807B6B7A0','_U3CU3Ec__DisplayClass1_0_U3COnUpdateU3Eb__3_mE150762AB6558CEF7143CD9C3C279572E014BFEB','_U3CU3Ec__DisplayClass1_0_U3COnUpdateU3Eb__6_m813BBA163868A33B4DAE8539FDFCC5D52B31C23B','_U3CU3Ec__DisplayClass10_0_U3CBuildDefaultRenderGraphU3Eb__0_m80D5EF2146B78BE359B2326D8088910872963A22','_U3CU3Ec__DisplayClass96_0_U3CDestroyAllTexturesU3Eb__0_mB59224FA6C05FEDF336E705B9F9723653B145321','_U3CU3Ec__DisplayClass96_0_U3CDestroyAllTexturesU3Eb__3_m6131FF7A9C9D31467614F6FECC9778A0917103CC','_U3CU3Ec__DisplayClass97_0_U3CShutdownU3Eb__0_m8DF3D9A115D50EE87DA0245E488F44FF02D1A289','_U3CU3Ec__DisplayClass127_0_U3CUploadTexturesU3Eb__0_m478525F5070B2CC99401EE3DF1205E50DE55B5D7','_U3CU3Ec__DisplayClass120_0_U3CUploadMeshesU3Eb__0_m5AA8ADF6FF638D80360588FC08723670EAED4B34','_U3CU3Ec__DisplayClass120_0_U3CUploadMeshesU3Eb__1_m50A47F69557D1F05655D8C08439B9DB60ADAF5C7','_U3CU3Ec__DisplayClass128_0_U3CUpdateRTTU3Eb__1_mE12234CDCA382F97D79BA995792FC83C487D363E','_U3CU3Ec__DisplayClass0_0_U3COnUpdateU3Eb__0_m776C73440E442A1E63B17752A6302A00D7E1DA0E','_U3CU3Ec_U3COnUpdateU3Eb__1_6_m0ADF5549955FFDBCA34483854B9B1AF9A81C3C1E','_StructuralChange_AddComponentEntitiesBatchExecute_mA9992EAFAB17A435D35C09B990AE5FAE52676A39','_StructuralChange_RemoveComponentEntitiesBatchExecute_m6632C5213792F71C74F594B1A5FE346C95533033','_StructuralChange_MoveEntityArchetypeExecute_m1FEF3D40A2CDF4B15AAF65BA953B04EADA5F5628','_U3CU3Ec__DisplayClass3_0_U3CInitializeSystemsU3Eb__0_m9719A5FE728EDE1FBF0C72105AC8544447F5CBED','_U3CU3Ec__DisplayClass3_0_U3CInitializeSystemsU3Eb__1_mF7CB925DD32BC2BD91BE2D76B4C5CB886FB40C07','_U3CU3Ec__DisplayClass_OnUpdate_LambdaJob0_PerformLambda_m87BE33CFD398760E10F74AFEFE10EF352F280A46','_U3CU3Ec__DisplayClass_OnUpdate_LambdaJob1_PerformLambda_mBE1855D34FA165EEBA9634C3F05A62C93A52382C','_U3CU3Ec__DisplayClass_OnUpdate_LambdaJob2_PerformLambda_m847B8710686A7AEBC61CECB1A7FC11F3475F04C2','_U3CU3Ec__DisplayClass1_0_U3COnUpdateU3Eb__0_mA39B449C7A2078637A42B949E02955ED9CD428AD','_U3CU3Ec__DisplayClass4_0_U3CFindCameraU3Eb__0_m27D9987C1502F10E5287A96C6223C8785DAFFE4A','_U3CU3Ec__DisplayClass4_0_U3CFindCameraU3Eb__1_m22EB15E590A8B5F55AEF94C4F0F08EF649CC2812','_U3CU3Ec__DisplayClass1_0_U3COnUpdateU3Eb__0_m7E49CE549BBA2FE2BC5E820ADE602F8290C9492E','_U3CU3Ec__DisplayClass1_1_U3COnUpdateU3Eb__2_mD57FDB20953DDB0A156660F2A364DDD8543EC1E6','_U3CU3Ec__DisplayClass0_0_U3COnUpdateU3Eb__3_m9064FC96520027D26E73C557781B5E2E1FD4006E','_U3CU3Ec__DisplayClass0_0_U3COnUpdateU3Eb__4_m7520874AD084443E8CCD4962D6F25197C3BA2B10','_AudioSystem_U3COnUpdateU3Eb__7_0_m0D5C2BBA8FAB6E90D2AEBA6488BB0CDE21B37074','_AudioSystem_U3COnUpdateU3Eb__7_1_mB3A33DD40BC8367BB86E91058B3A3C2E0821DFFC','_AudioSystem_U3COnUpdateU3Eb__7_2_m625D733B21AF3CDB4D2F660948681FBE960AFB88','_AudioSystem_U3COnUpdateU3Eb__7_3_mBAAD9D1912ECADF15D54A08A3AD779A615EEDDCE','__ZL13capture_frameP25bgfx_callback_interface_sPKvj','__ZN4bgfx2glL25stubInvalidateFramebufferEjiPKj','_emscripten_glBindAttribLocation','_emscripten_glDrawArrays','_emscripten_glGetBufferParameteriv','_emscripten_glGetProgramiv','_emscripten_glGetRenderbufferParameteriv','_emscripten_glGetShaderiv','_emscripten_glGetTexParameterfv','_emscripten_glGetTexParameteriv','_emscripten_glGetUniformfv','_emscripten_glGetUniformiv','_emscripten_glGetVertexAttribfv','_emscripten_glGetVertexAttribiv','_emscripten_glGetVertexAttribPointerv','_emscripten_glStencilFunc','_emscripten_glStencilOp','_emscripten_glTexParameterfv','_emscripten_glTexParameteri','_emscripten_glTexParameteriv','_emscripten_glUniform1fv','_emscripten_glUniform1iv','_emscripten_glUniform2fv','_emscripten_glUniform2i','_emscripten_glUniform2iv','_emscripten_glUniform3fv','_emscripten_glUniform3iv','_emscripten_glUniform4fv','_emscripten_glUniform4iv','_emscripten_glGetQueryivEXT','_emscripten_glGetQueryObjectivEXT','_emscripten_glGetQueryObjectuivEXT','_emscripten_glGetQueryObjecti64vEXT','_emscripten_glGetQueryObjectui64vEXT','_emscripten_glGetQueryiv','_emscripten_glGetQueryObjectuiv','_emscripten_glGetBufferPointerv','_emscripten_glFlushMappedBufferRange','_emscripten_glGetIntegeri_v','_emscripten_glBindBufferBase','_emscripten_glGetVertexAttribIiv','_emscripten_glGetVertexAttribIuiv','_emscripten_glGetUniformuiv','_emscripten_glUniform2ui','_emscripten_glUniform1uiv','_emscripten_glUniform2uiv','_emscripten_glUniform3uiv','_emscripten_glUniform4uiv','_emscripten_glClearBufferiv','_emscripten_glClearBufferuiv','_emscripten_glClearBufferfv','_emscripten_glUniformBlockBinding','_emscripten_glGetInteger64i_v','_emscripten_glGetBufferParameteri64v','_emscripten_glSamplerParameteri','_emscripten_glSamplerParameteriv','_emscripten_glSamplerParameterfv','_emscripten_glGetSamplerParameteriv','_emscripten_glGetSamplerParameterfv','_emscripten_glProgramParameteri','_emscripten_glInvalidateFramebuffer',0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0];
var debug_table_viiii = [0,'_F_DDD_3_Invoke_mD1CE1ECEE13E591DAE84E583C98728B92B83B61D','_Image2DIOHTMLLoader_FreeNative_m5CB30C270ADBBB068EEEFD32071A7ABAB9F58BCF','_PerformLambdaDelegate_Invoke_m98AA3543BF21BE985F4CC17C9DD5C1BF67E9C664','_AddComponentEntitiesBatchDelegate_Invoke_m81A8D5E64C1513E4056FDDA33E03C9FD746F8FBC','_RemoveComponentEntitiesBatchDelegate_Invoke_m1F4ACE6C740AAF68C33F3A01FF6C0AB4AFC94AEA','_MoveEntityArchetypeDelegate_Invoke_m871D0F6874B4B28CFF7E4DB27703E527E09BC7A0','_AudioHTMLSystemLoadFromFile_FreeNative_m70ACE5494AA412C30FBB7754D9B6220B4670676A','_F_EDD_2_Invoke_m050F44EF0C0577FAF41D8AA1967DF13690468C78','_U3CU3Ec__DisplayClass_OnUpdate_LambdaJob0_Execute_mC2BB395EED35F7632FF9DA2B34728209A2779725_AdjustorThunk','_U3CU3Ec__DisplayClass_OnUpdate_LambdaJob0_Execute_mB6F4C7F1B4E668DC7231674A85015E398DB0CD99_AdjustorThunk','_U3CU3Ec__DisplayClass_OnUpdate_LambdaJob0_Execute_m6DD77662740AADDA44B2659D84A9651AAB5E731F_AdjustorThunk','_U3CU3Ec__DisplayClass_OnUpdate_LambdaJob0_Execute_m86EE16BDD08A81B3C459AD13CF4C16CFFBA7D94E_AdjustorThunk','_U3CU3Ec__DisplayClass_OnUpdate_LambdaJob0_Execute_m4BEE5A90E854C93661D8680671B54659FFC046CD_AdjustorThunk','_U3CU3Ec__DisplayClass_OnUpdate_LambdaJob0_Execute_mD5D8AF46619D05F73F682F7A027E5AF073F40D7A_AdjustorThunk','_U3CU3Ec__DisplayClass_OnUpdate_LambdaJob0_Execute_mEEBAF807CBA26B9AB79A92E331FC25315436E7BB_AdjustorThunk','_U3CU3Ec__DisplayClass_OnUpdate_LambdaJob0_Execute_m7D7673052F0D358F965CFEEFE00A32E4D2D0D8D8_AdjustorThunk','_U3CU3Ec__DisplayClass_OnUpdate_LambdaJob0_Execute_m6274F5C9545A8B9928A341809896DF51084E9356_AdjustorThunk','_GatherComponentDataJob_1_Execute_mB81000375BA9E1867C5DDD3EADF12E2348A8591A_AdjustorThunk','_GatherEntitiesJob_Execute_mFB02F83EE5235B6ED4753C1E826AC5B14B4BDE69_AdjustorThunk','_SubmitSimpleLitMeshJob_Execute_m5CBC5774C0068E1593CA99850BEE95168FE170DB_AdjustorThunk','_BuildEntityGuidHashMapJob_Execute_m176DA17ACEF9AC0AAC258EB8431A0E1F943914F1_AdjustorThunk','_ToCompositeRotation_Execute_m2D54CF99DABBE5DD9614200125EF039A6604F2F4_AdjustorThunk','_ToCompositeScale_Execute_m002B6B5DEEF1837296598C74134E261A62BDCB4B_AdjustorThunk','_UpdateHierarchy_Execute_mED64DF77AFD4A2AC0D0B70E7B1D90384CA49DC74_AdjustorThunk','_ToChildParentScaleInverse_Execute_m8C1627A557AE21DE9B7E7523AFB14FA16294F9F5_AdjustorThunk','_GatherChangedParents_Execute_mFC220C1E9BAF3A74AE87331854B9892FAB12ADFB_AdjustorThunk','_PostRotationEulerToPostRotation_Execute_mC96EA04B5309C98D418D2941A80D6779DD0A6B31_AdjustorThunk','_RotationEulerToRotation_Execute_m4DA8C0204AC1B32523C931D8B86470D5E6B5EA5E_AdjustorThunk','_TRSToLocalToParent_Execute_m185A564D77B1131331065663330F199074D0718B_AdjustorThunk','_TRSToLocalToWorld_Execute_mD3A5E2DECDE932BB8B1C3FECD3F6928B896D9C93_AdjustorThunk','_ToWorldToLocal_Execute_m6F5BBD2C72D7E3E369AF7D0CFA85514BEFC06E52_AdjustorThunk','__ZN4bgfx2gl17RendererContextGL17createIndexBufferENS_17IndexBufferHandleEPKNS_6MemoryEt','__ZN4bgfx2gl17RendererContextGL24createDynamicIndexBufferENS_17IndexBufferHandleEjt','__ZN4bgfx2gl17RendererContextGL25createDynamicVertexBufferENS_18VertexBufferHandleEjt','__ZN4bgfx2gl17RendererContextGL13createProgramENS_13ProgramHandleENS_12ShaderHandleES3_','__ZN4bgfx2gl17RendererContextGL18updateTextureBeginENS_13TextureHandleEhh','__ZN4bgfx2gl17RendererContextGL11readTextureENS_13TextureHandleEPvh','__ZN4bgfx2gl17RendererContextGL17createFrameBufferENS_17FrameBufferHandleEhPKNS_10AttachmentE','__ZN4bgfx2gl17RendererContextGL13updateUniformEtPKvj','__ZN4bgfx2gl17RendererContextGL7setNameENS_6HandleEPKct','__ZN4bgfx2gl17RendererContextGL6submitEPNS_5FrameERNS_9ClearQuadERNS_19TextVideoMemBlitterE','__ZN4bgfx4noop19RendererContextNOOP17createIndexBufferENS_17IndexBufferHandleEPKNS_6MemoryEt','__ZN4bgfx4noop19RendererContextNOOP24createDynamicIndexBufferENS_17IndexBufferHandleEjt','__ZN4bgfx4noop19RendererContextNOOP25createDynamicVertexBufferENS_18VertexBufferHandleEjt','__ZN4bgfx4noop19RendererContextNOOP13createProgramENS_13ProgramHandleENS_12ShaderHandleES3_','__ZN4bgfx4noop19RendererContextNOOP18updateTextureBeginENS_13TextureHandleEhh','__ZN4bgfx4noop19RendererContextNOOP11readTextureENS_13TextureHandleEPvh','__ZN4bgfx4noop19RendererContextNOOP17createFrameBufferENS_17FrameBufferHandleEhPKNS_10AttachmentE','__ZN4bgfx4noop19RendererContextNOOP13updateUniformEtPKvj','__ZN4bgfx4noop19RendererContextNOOP7setNameENS_6HandleEPKct','__ZN4bgfx4noop19RendererContextNOOP6submitEPNS_5FrameERNS_9ClearQuadERNS_19TextVideoMemBlitterE','__ZNK10__cxxabiv117__class_type_info27has_unambiguous_public_baseEPNS_19__dynamic_cast_infoEPvi','__ZNK10__cxxabiv120__si_class_type_info27has_unambiguous_public_baseEPNS_19__dynamic_cast_infoEPvi','_U3CU3Ec__DisplayClass0_0_U3COnUpdateU3Eb__1_mAE9B89998F50B4380E3789D20BA35C71C48EA4BE','_U3CU3Ec__DisplayClass5_0_U3COnUpdateU3Eb__1_m9C1F3E49E5988926D3D10906335D54F400A49B53','_U3CU3Ec__DisplayClass3_0_U3COnUpdateU3Eb__0_m3A79D4C4E0854BDB3B0AE25A4B0F27BA4CF31AE7','_U3CU3Ec__DisplayClass5_0_U3COnUpdateU3Eb__0_mF863D8FFFDD5DAB07A3D02D7A935F4996EE792BF','_U3CU3Ec__DisplayClass1_0_U3COnUpdateU3Eb__4_mB12BD304A1A5DE24317695F1F5B56AE9BD55A083','_U3CU3Ec__DisplayClass1_0_U3COnUpdateU3Eb__5_mFF3E0D8F621DDF9FEEBA0E27C1923386F03FA568','_U3CU3Ec__DisplayClass14_0_U3CBuildAllLightNodesU3Eb__0_mBDDF6D193B05B2A14136AFC168D836481D44B2B4','_U3CU3Ec_U3CUpdateExternalTexturesU3Eb__123_0_m7ADB410BA28D55D28D781500F6A1430674BA004B','_U3CU3Ec__DisplayClass127_0_U3CUploadTexturesU3Eb__1_mB9A4FAEB7F9C6054B8F9C816B8742487AE3D112C','_U3CU3Ec__DisplayClass128_0_U3CUpdateRTTU3Eb__0_m3D06490282ECAB45A4FBD412C5467CEA8FB3F194','_U3CU3Ec__DisplayClass5_0_U3COnUpdateU3Eb__3_m1A53D70701E6BE7150A49AD758628B4B405F4D93','_U3CU3Ec__DisplayClass1_0_U3COnUpdateU3Eb__1_m8667FFB66CEAA16D3026D8F547F62844666E1C20','_U3CU3Ec__DisplayClass5_0_U3COnUpdateU3Eb__2_m20CEF4A43E3C8A76A394F843BECB40A5518DBC69','_U3CU3Ec__DisplayClass5_0_U3COnUpdateU3Eb__3_mF7A5234B57925D66BC1912AC5A0A1A1F28F298C3','_StructuralChange_AddComponentChunksExecute_m93FADB4248E9D744F87C5BA0A92F6D85F9C87720','_StructuralChange_RemoveComponentChunksExecute_m884C1F67D3E5366A235EFFF73BECAD43451251AE','_StructuralChange_CreateEntityExecute_m004B3E705017E2710FF182143178D852D16D08AB','_StructuralChange_InstantiateEntitiesExecute_mCC1E269F8C1720814E7F240E61D755E9E7B4AE5F','_U3CU3Ec_U3COnUpdateU3Eb__2_3_m44FD77C0F2F0CF7F99DB1A55C4AC0C1ECD1D6CFB','_U3CU3Ec__DisplayClass0_0_U3COnUpdateU3Eb__1_m48A22216FA0435EE5098FDBDEB682E6011ED828C','_U3CU3Ec__DisplayClass0_0_U3COnUpdateU3Eb__2_m3BD60A1F0BD821A262CF6FFE30BF0E6A7D5CC8AF','__ZN4bgfx2glL27stubMultiDrawArraysIndirectEjPKvii','__ZN4bgfx2glL23stubDrawArraysInstancedEjiii','__ZN4bgfx2glL18stubPushDebugGroupEjjiPKc','__ZN4bgfx2glL15stubObjectLabelEjjiPKc','_emscripten_glBlendFuncSeparate','_emscripten_glBufferData','_emscripten_glBufferSubData','_emscripten_glColorMask','_emscripten_glDrawElements','_emscripten_glFramebufferRenderbuffer','_emscripten_glGetAttachedShaders','_emscripten_glGetFramebufferAttachmentParameteriv','_emscripten_glGetProgramInfoLog','_emscripten_glGetShaderInfoLog','_emscripten_glGetShaderPrecisionFormat','_emscripten_glGetShaderSource','_emscripten_glRenderbufferStorage','_emscripten_glScissor','_emscripten_glShaderSource','_emscripten_glStencilFuncSeparate','_emscripten_glStencilOpSeparate','_emscripten_glUniform3i','_emscripten_glUniformMatrix2fv','_emscripten_glUniformMatrix3fv','_emscripten_glUniformMatrix4fv','_emscripten_glViewport','_emscripten_glDrawArraysInstancedANGLE','_emscripten_glUniformMatrix2x3fv','_emscripten_glUniformMatrix3x2fv','_emscripten_glUniformMatrix2x4fv','_emscripten_glUniformMatrix4x2fv','_emscripten_glUniformMatrix3x4fv','_emscripten_glUniformMatrix4x3fv','_emscripten_glTransformFeedbackVaryings','_emscripten_glUniform3ui','_emscripten_glGetUniformIndices','_emscripten_glGetActiveUniformBlockiv','_emscripten_glDrawArraysInstanced','_emscripten_glProgramBinary','_emscripten_glDrawArraysInstancedNV','_emscripten_glDrawArraysInstancedEXT','_emscripten_glDrawArraysInstancedARB',0,0,0,0,0,0,0,0,0,0,0];
var debug_table_viiiii = [0,'_F_DDDD_4_Invoke_m7D07A1EF426B56911D366AB20878FFF0FC945719','_F_DDDD_4_Invoke_m9BA76BD1E0214D9898584CE67BD06D3A446F590D','_F_EDDD_3_Invoke_mFAAB9A6BF7EB09AFDE2358469E3C97E1091E7FDC','_F_EDDD_3_Invoke_mC9E5BD49FBD8FBA0EE154D0E773717EC4F600D6D','_AddComponentChunksDelegate_Invoke_mEB39B42D8E8A764C07CF99AFA4F0E25F7E9832D3','_RemoveComponentChunksDelegate_Invoke_m2D471B50C0243AC46440B324DBBF3897D967D068','_CreateEntityDelegate_Invoke_m350507B1E9396D0E97C268DD5D3658D1C9CE5A31','_InstantiateEntitiesDelegate_Invoke_mBEA19C2146BAE848974391288BA3B44F83A2006B','__ZN4bgfx2gl17RendererContextGL18createVertexBufferENS_18VertexBufferHandleEPKNS_6MemoryENS_18VertexLayoutHandleEt','__ZN4bgfx2gl17RendererContextGL24updateDynamicIndexBufferENS_17IndexBufferHandleEjjPKNS_6MemoryE','__ZN4bgfx2gl17RendererContextGL25updateDynamicVertexBufferENS_18VertexBufferHandleEjjPKNS_6MemoryE','__ZN4bgfx2gl17RendererContextGL13createUniformENS_13UniformHandleENS_11UniformType4EnumEtPKc','__ZN4bgfx4noop19RendererContextNOOP18createVertexBufferENS_18VertexBufferHandleEPKNS_6MemoryENS_18VertexLayoutHandleEt','__ZN4bgfx4noop19RendererContextNOOP24updateDynamicIndexBufferENS_17IndexBufferHandleEjjPKNS_6MemoryE','__ZN4bgfx4noop19RendererContextNOOP25updateDynamicVertexBufferENS_18VertexBufferHandleEjjPKNS_6MemoryE','__ZN4bgfx4noop19RendererContextNOOP13createUniformENS_13UniformHandleENS_11UniformType4EnumEtPKc','__ZN4bgfx12CallbackStub5fatalEPKctNS_5Fatal4EnumES2_','__ZN4bgfx12CallbackStub10traceVargsEPKctS2_Pi','__ZN4bgfx12CallbackStub13profilerBeginEPKcjS2_t','__ZN4bgfx12CallbackStub20profilerBeginLiteralEPKcjS2_t','__ZN4bgfx11CallbackC995fatalEPKctNS_5Fatal4EnumES2_','__ZN4bgfx11CallbackC9910traceVargsEPKctS2_Pi','__ZN4bgfx11CallbackC9913profilerBeginEPKcjS2_t','__ZN4bgfx11CallbackC9920profilerBeginLiteralEPKcjS2_t','__ZNK10__cxxabiv117__class_type_info16search_below_dstEPNS_19__dynamic_cast_infoEPKvib','__ZNK10__cxxabiv120__si_class_type_info16search_below_dstEPNS_19__dynamic_cast_infoEPKvib','_JobChunk_Process_1_Execute_m9B410E035684630261B18D6A549566284303FB9B','_JobChunk_Process_1_Execute_mCDF3E12C313E2743F6F1B73986E066D73F4D39C8','_JobChunk_Process_1_Execute_m0637D70EC0E6286F20E43111307E8B9CC5CEECD9','_JobChunk_Process_1_Execute_m1BAEBAD6CEE5235E830982B5E49ED01A6F87BD7C','_JobChunk_Process_1_Execute_m1573156B4B628BB346E84F0205153BDB90E08E70','_JobChunk_Process_1_Execute_mAA00EA4B1F6B2742541970F6F28A917BC3791A77','_JobChunk_Process_1_Execute_m159AC516205C58739A80032EE49DA7CFC9CB1C11','_JobChunk_Process_1_Execute_m673B173A26F4348BFEB9FCCC3C0B1F6B9F46CA6A','_JobChunk_Process_1_Execute_m71AEE1991BEA6B0430FA1BD759AF981AB687EE6C','_JobChunk_Process_1_Execute_m801FB8D6909A462056A2B578CFFAC7A5DCC5DC3F','_JobChunk_Process_1_Execute_m2571C758ED692B6A17789506593234168182DB59','_JobChunk_Process_1_Execute_m2ED4F3C3CD27DB09F4D97243D4AA2F23EB500808','_JobChunk_Process_1_Execute_m5D4ED18DE4B49308D6308C082E42BDDFED921A3C','_JobChunk_Process_1_Execute_mE96DBF8EC0825F2356417FF372A7E8F31B6B73E3','_JobChunk_Process_1_Execute_m15419450A1D73B379C49E69A841A371F7176C880','_JobChunk_Process_1_Execute_m8A765B35BC5A70B01866BDF538059DA4DABCEF7B','_JobChunk_Process_1_Execute_mDE4543CE13F45A5C8CB2AAFC32E1484848322F18','_JobChunk_Process_1_Execute_m4707176088513A91AB92D53437574159710ACCD7','_JobChunk_Process_1_Execute_m2B2CD3ACC71F7B8EE5B67BEE51FD520FA99FBEE5','_JobChunk_Process_1_Execute_m38BA88BDF86DE54E47DEA3077B2A9C5CB9764CCE','_JobChunk_Process_1_Execute_m921A3954C5157329B65CE373ACDFDD36D62F69EC','_JobChunk_Process_1_Execute_m93E00C7E46A82CAFB399F552C96EFDFCF515C23E','_JobChunk_Process_1_Execute_m11BEDF80846B55F83455849B402A0BBAF96C3799','_JobStruct_1_Execute_m4988D1031607AFD6FAD37ECC418A0B94E770AD22','_JobStruct_1_Execute_m4C5FFD94C8D231D0AE66F742D4DC582555069B9A','_JobStruct_1_Execute_m94454792A519167008212833E02852DB1B847CD6','_JobStruct_1_Execute_mF89E54DE9B96050C2C159FB4DC9BADE32D920430','_JobStruct_1_Execute_mBCAEB96372BEF01CA3FA9D96CE443F4CFD6EB5A5','_JobStruct_1_Execute_m18A491D2FE3823EB834C3105C90BC47622254B40','_JobStruct_1_Execute_mF534C5F5F8F4E1ACA0968E24CA79C67AC17BE606','_JobStruct_1_Execute_m853EB2F30B4A3750EE7F95E35C684FF26ADA52AB','_JobStruct_1_Execute_m3C394352CF90EEF8B3D46999A735B873D44F653B','_JobStruct_1_Execute_mBFA2D4E385B7F360662EC85385E2F66C9E33E6B7','_JobStruct_1_Execute_mE1BDBAB8E73B1E28B5A80CEEF5BD831A33C07AA2','_ParallelForJobStruct_1_Execute_mC93D7295FFB49A2CF17FBB1F3A2E1C6FECE6C0B9','_ParallelForJobStruct_1_Execute_mD50C0DDE80671FB0BC182E81111C2D7422832541','_ParallelForJobStruct_1_Execute_m6556FE408528DC275553A0CE36A53651EAF4C350','_ParallelForJobStruct_1_Execute_m2C2132369A26C139319FED0558038AE1F87C5A7D','_ParallelForJobStruct_1_Execute_m2FDEB6CF0E54711136CA3ECB0BBC078DA7D5DDE9','_ParallelForJobStruct_1_Execute_m4314DDF52A8A439DED53CA4B71BB30D0F2C5298F','_ParallelForJobStruct_1_Execute_mE1A36BE7D21119F5D681F448FE40A85D8703BF9A','_ParallelForJobStruct_1_Execute_mA89D7700455109EBC02F97A192D91160D1D31CFF','_U3CU3Ec__DisplayClass2_0_U3COnUpdateU3Eb__2_mCCFC500CF37DC1B3C55836B476CB67018B0C3ED9','_U3CU3Ec__DisplayClass2_0_U3COnUpdateU3Eb__2_mB9192C849F8875D42E51B94DAC33E11559BC7BD0','_EnterBoostPad_U3COnUpdateU3Eb__0_0_m106E5409421E88409B63576E336CD430995B4819','_U3CU3Ec__DisplayClass2_0_U3COnUpdateU3Eb__0_mA5CB008664E7B83171D712154C4777C1F35C6D60','_U3CU3Ec__DisplayClass5_0_U3COnUpdateU3Eb__1_m5747F8EF3CEC2B1E158471F74DEDCB21BD955F64','_U3CU3Ec__DisplayClass5_0_U3COnUpdateU3Eb__2_mAADD3C5204127D78474F870A2AF12D589DFBD12A','_U3CU3Ec__DisplayClass5_0_U3COnUpdateU3Eb__4_mA939170879434BA7CD8C53F20D4DDF3FB349BE3B','_U3CU3Ec__DisplayClass1_0_U3COnUpdateU3Eb__3_m3C9B9F06D05BEB8EA98D108AEF958705F916E498','_StructuralChange_AddSharedComponentChunksExecute_mDE42CA5BEB4AA2BD8D338F87AAE78260366C4C69','_StructuralChange_SetChunkComponentExecute_m2C93664388AEC82B9530D7B83D4A5D30BA04AB90','_U3CU3Ec__DisplayClass5_0_U3COnUpdateU3Eb__0_mF493768363F5D07FC825887ACE82E7B87242BFE7','_U3CU3Ec_U3COnUpdateU3Eb__2_2_m7321023A1B663304F2E2CF7968DC40BCF503C8DE','_U3CU3Ec_U3COnUpdateU3Eb__0_4_m80C9EA9FC0FA6DDA241A2557DD169963016C8D40','_U3CU3Ec_U3COnUpdateU3Eb__0_5_m65E29A5FC31C1262B4523022C0A87B933FC5279E','_U3CU3Ec_U3COnUpdateU3Eb__0_6_m636627C8FDE65C5D7321489EC2571728F27FF4EA','_U3CU3Ec_U3COnUpdateU3Eb__0_7_mB57412808EA7509A60FB1AFB9D6B83FFAC77135D','__ZL5fatalP25bgfx_callback_interface_sPKct10bgfx_fatalS2_','__ZL11trace_vargsP25bgfx_callback_interface_sPKctS2_Pi','__ZL14profiler_beginP25bgfx_callback_interface_sPKcjS2_t','__ZL22profiler_begin_literalP25bgfx_callback_interface_sPKcjS2_t','__ZN4bgfx2glL29stubMultiDrawElementsIndirectEjjPKvii','__ZN4bgfx2glL25stubDrawElementsInstancedEjijPKvi','_emscripten_glFramebufferTexture2D','_emscripten_glShaderBinary','_emscripten_glUniform4i','_emscripten_glDrawElementsInstancedANGLE','_emscripten_glRenderbufferStorageMultisample','_emscripten_glFramebufferTextureLayer','_emscripten_glBindBufferRange','_emscripten_glVertexAttribIPointer','_emscripten_glVertexAttribI4i','_emscripten_glVertexAttribI4ui','_emscripten_glUniform4ui','_emscripten_glCopyBufferSubData','_emscripten_glGetActiveUniformsiv','_emscripten_glGetActiveUniformBlockName','_emscripten_glDrawElementsInstanced','_emscripten_glGetSynciv','_emscripten_glGetProgramBinary','_emscripten_glTexStorage2D','_emscripten_glGetInternalformativ','_emscripten_glDrawElementsInstancedNV','_emscripten_glDrawElementsInstancedEXT','_emscripten_glDrawElementsInstancedARB',0,0,0,0,0,0,0,0,0,0,0,0,0,0,0];
var debug_table_viiiiii = [0,'_F_DDDDD_5_Invoke_m576484E485F35F7CF614E030D7829889CD4CD184','_F_DDDDD_5_Invoke_m9D4ADF5357BD7214595D7F3492167587F4D97452','_Image2DIOHTMLLoader_FinishLoading_m8532AAD7D5A19020A0D4F22554CBCA046CC05D00','_AddSharedComponentChunksDelegate_Invoke_m69D258DA9173E9C6C810047D548EAF5F3EE57867','_SetChunkComponentDelegate_Invoke_m6628766D30D9BD728BDDC92E544F7760E4671C29','_AudioHTMLSystemLoadFromFile_FinishLoading_m755700D228BCBC82DA7205CAB8B08DD3170A5C00','_ExecuteJobFunction_Invoke_m9AEB2A782FD1B0D5DC5B2BC9F5A5692E54008DA8','_ExecuteJobFunction_Invoke_m1BEE4E4326C62A2186936097126C7364ADFC50AF','_ExecuteJobFunction_Invoke_m5A893A946D50ECBFFFBB045AD1507E15831D2D9D','_ExecuteJobFunction_Invoke_m7F74F4D226D0E93D2E98279A11BD9511E899FF75','_ExecuteJobFunction_Invoke_m591E82FA04B092442F392FACD5CFF034D6C953EE','_ExecuteJobFunction_Invoke_mC1BF41060DB07902042B4CB3844966918C8ED8CA','_ExecuteJobFunction_Invoke_mDC9A6FC3D729CC1B58831603E694469025D2E620','_ExecuteJobFunction_Invoke_m5AC4CE9A0288013B01260A27DD01DF503A6C6DA2','_ExecuteJobFunction_Invoke_mDE36FCD9DF329073EEF420A1AECBEE1B09AC072B','_ExecuteJobFunction_Invoke_m63D337CCD35951CE0490FDF27D0D4910CE84A3F9','_ExecuteJobFunction_Invoke_mB2E14136A387F2A88FF95C3D6C8EFB84BDAAF7FB','_ExecuteJobFunction_Invoke_m76A4BFCC80D6417707F382C3BBC11746667DE3AD','_ExecuteJobFunction_Invoke_m30744A381142B92C96120931F8484F5D10FDEA5A','_ExecuteJobFunction_Invoke_mF21E5C2E2575450AE1D7CFF9246C422914EA6D84','_ExecuteJobFunction_Invoke_m20D57096E6F2036D4F7BC85253C0CD86596F5FE8','_ExecuteJobFunction_Invoke_m262700A3FCA743B344EE603292E862130452CD60','_ExecuteJobFunction_Invoke_mFDD795676503D3F9AFC3F3C85B113B16ED97E02C','_ExecuteJobFunction_Invoke_m5A70AC2F6A03CBAFF8A911BAEE2D85B959A40236','_ExecuteJobFunction_Invoke_m1ACFB526E18CF88043B3E7E55F8B01963BFA500A','_ExecuteJobFunction_Invoke_m4B1842961DEE66D37B866AFD8CAA4DBB98206489','_ExecuteJobFunction_Invoke_mD52BDE08B5EE63C565F3ECCE84006FDBEA41421F','_ExecuteJobFunction_Invoke_m529DD09ADDDE8C905373C91B946AA809719D1D9A','_ExecuteJobFunction_Invoke_m707BE01F6262C530A5D5DF753079F721903A65CF','_ExecuteJobFunction_Invoke_m804F13257C41C7C9C250581903E82476B101B511','_ExecuteJobFunction_Invoke_m2AE0515EE401AF32469AC27EA2708CD252789211','_ExecuteJobFunction_Invoke_m6BED8BBB275833F7C32E371483AFA06718818E15','_ExecuteJobFunction_Invoke_m95A6B244B61F79D2C789D024A78CBCCF3FA1825F','_ExecuteJobFunction_Invoke_m4CA8317AD8C5D53C9090BA9811921F65AC76FDC1','_ExecuteJobFunction_Invoke_mBA43781008CB3213D49E85D790E7CF9A8C34ED98','_ExecuteJobFunction_Invoke_m28F04B43358A70A120AAB3298C7BFF4B1DE51617','_ExecuteJobFunction_Invoke_m0410192684D4042A8A38EAC1FE5DFC1DC57ED40F','_ExecuteJobFunction_Invoke_m1EAE6982C4B1E35542AEBC52D863E63B548427FF','_ExecuteJobFunction_Invoke_m9BE292287181C7F7B5997808CBB5671A81FB77E5','_ExecuteJobFunction_Invoke_m88BA2D5BB4ED3CEA4529128191ACC33796B2F611','_ExecuteJobFunction_Invoke_m213E5C9E6917103C8B267AA83502ED5881582CEA','_ExecuteJobFunction_Invoke_mFD6B2A1DA72FBCC53EEE192D598E95F850421E5D','_ExecuteJobFunction_Invoke_m95C533DCCB59E826B059AF5275AE6083C2D71AF1','_ExecuteJobFunction_Invoke_m5BEA1405A603F8B7B755573D4BD21DCDCD86CC57','_ExecuteJobFunction_Invoke_mD293FBF0A7A68568E0A6AC3F5EAEFEBC956D5405','_ExecuteJobFunction_Invoke_m81A417EEC8E8A62B010D13AEB90C4A32CD8509C5','_ExecuteJobFunction_Invoke_mA0546395B25011253B258F5295FE2BDD3DD50233','_ExecuteJobFunction_Invoke_m6FE853F385385B00CF697ECC30BADDADB29C93F8','_ExecuteJobFunction_Invoke_m8EF39FFD7601779047C8092857C8399392188F54','_ExecuteJobFunction_Invoke_mA938C823B720C0D223AE38C222AFBBD8C6894403','_ExecuteJobFunction_Invoke_m68E419C688A3E6E32C0434E7AD9B2151021C747D','__ZN4bgfx2gl17RendererContextGL13resizeTextureENS_13TextureHandleEttht','__ZN4bgfx4noop19RendererContextNOOP13resizeTextureENS_13TextureHandleEttht','__ZN4bgfx12CallbackStub12captureBeginEjjjNS_13TextureFormat4EnumEb','__ZN4bgfx11CallbackC9912captureBeginEjjjNS_13TextureFormat4EnumEb','__ZNK10__cxxabiv117__class_type_info16search_above_dstEPNS_19__dynamic_cast_infoEPKvS4_ib','__ZNK10__cxxabiv120__si_class_type_info16search_above_dstEPNS_19__dynamic_cast_infoEPKvS4_ib','_U3CU3Ec__DisplayClass2_0_U3COnUpdateU3Eb__3_m0E2AE7E10F2BA888620C4B5EB084CA4FFEBA48B0','_U3CU3Ec__DisplayClass2_0_U3COnUpdateU3Eb__3_m06DED4FC9F867B3B80E26483429EC851D8913557','_U3CU3Ec__DisplayClass5_0_U3COnUpdateU3Eb__0_mA14AAE3C0D2628931866E561DB8C6ECB4F3E8DA6','_U3CU3Ec__DisplayClass5_0_U3COnUpdateU3Eb__0_mCE366DDA320ECCD52497358AB7D99F83E895C3F5','_U3CU3Ec__DisplayClass5_0_U3COnUpdateU3Eb__5_mD8A6A397139516F8CA68886DD5A7DE16AEFF3D2F','_U3CU3Ec__DisplayClass5_0_U3COnUpdateU3Eb__6_mB72E0E4C2713BAB44737281573DDA3D4F0D6833E','_U3CU3Ec__DisplayClass1_0_U3COnUpdateU3Eb__2_m6CC0AF223F4D76CFAE8FB3CC3E2543465B7E13AA','_U3CU3Ec__DisplayClass1_0_U3COnUpdateU3Eb__1_m07F088155110352443891FB846561D682308D5B4','_U3CU3Ec_U3COnUpdateU3Eb__0_2_mCA0DD9776DD5875F81412F69F1F8719221D1D208','_U3CU3Ec_U3COnUpdateU3Eb__0_3_m2BCED6195898404A128CBB15665FEB93A7E719F0','__ZL13capture_beginP25bgfx_callback_interface_sjjj19bgfx_texture_formatb','_emscripten_glVertexAttribPointer','_emscripten_glDrawRangeElements','_emscripten_glTexStorage3D',0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0];
var debug_table_viiiiiii = [0,'_Image2DIOHTMLLoader_StartLoad_m2AA96C68AB0A9EC323F9324A270B5D16F9145B9E','_AudioHTMLSystemLoadFromFile_StartLoad_mBD7FDB346766DB8F4920B2CAC870FF1FB28E48A2','_F_EDDDDD_5_Invoke_m777592CCDDCA77FFC4312BEC582176AC8BED3AE4','__ZN4bgfx2gl17RendererContextGL17createFrameBufferENS_17FrameBufferHandleEPvjjNS_13TextureFormat4EnumES5_','__ZN4bgfx4noop19RendererContextNOOP17createFrameBufferENS_17FrameBufferHandleEPvjjNS_13TextureFormat4EnumES5_','_U3CU3Ec_U3COnUpdateU3Eb__5_1_m7FE245C8713D9B0F6A79A60D81CDFFD49939C3B1','_U3CU3Ec__DisplayClass0_0_U3COnUpdateU3Eb__0_mF357A248E8A2A39BF4832337A4D7AF2922F3117D','_SendMessageHandler_OnSendMessage_m5ABCD9BF9AC11BEC3D9421A7BCB8B56D7069CE55','_ReversePInvokeWrapper_SendMessageHandler_OnSendMessage_m5ABCD9BF9AC11BEC3D9421A7BCB8B56D7069CE55','__ZN4bgfx2gl11debugProcCbEjjjjiPKcPKv','_emscripten_glGetActiveAttrib','_emscripten_glGetActiveUniform','_emscripten_glReadPixels','_emscripten_glGetTransformFeedbackVarying','_emscripten_glInvalidateSubFramebuffer'];
var debug_table_viiiiiiii = [0,'_RegisterSendMessageDelegate_Invoke_m3D20C4DCE61F24BC16D6CFB014D0A86841CC8769','__ZN4bgfx12CallbackStub10screenShotEPKcjjjPKvjb','__ZN4bgfx11CallbackC9910screenShotEPKcjjjPKvjb','_U3CU3Ec__DisplayClass1_0_U3COnUpdateU3Eb__4_mD9DF99810A1A6FA735BCBCC948B2215EF216B088','_U3CU3Ec__DisplayClass1_0_U3COnUpdateU3Eb__5_m91051E1E0861E56C31FAAA7197C68563AC514076','__ZL11screen_shotP25bgfx_callback_interface_sPKcjjjPKvjb','_emscripten_glCompressedTexImage2D','_emscripten_glCopyTexImage2D','_emscripten_glCopyTexSubImage2D',0,0,0,0,0,0];
var debug_table_viiiiiiiii = [0,'__ZN4bgfx2gl17RendererContextGL13updateTextureENS_13TextureHandleEhhRKNS_4RectEtttPKNS_6MemoryE','__ZN4bgfx4noop19RendererContextNOOP13updateTextureENS_13TextureHandleEhhRKNS_4RectEtttPKNS_6MemoryE','_emscripten_glCompressedTexSubImage2D','_emscripten_glTexImage2D','_emscripten_glTexSubImage2D','_emscripten_glCopyTexSubImage3D','_emscripten_glCompressedTexImage3D'];
var debug_table_viiiiiiiiii = [0,'_emscripten_glTexImage3D','_emscripten_glBlitFramebuffer',0];
var debug_table_viiiiiiiiiii = [0,'_emscripten_glTexSubImage3D','_emscripten_glCompressedTexSubImage3D',0];
var debug_table_viiiiiiiiiiiiiii = [0];
var debug_table_viij = [0,'_emscripten_glWaitSync'];
var debug_table_vijii = [0,'__ZN4bgfx12CallbackStub10cacheWriteEyPKvj','__ZN4bgfx11CallbackC9910cacheWriteEyPKvj','__ZL11cache_writeP25bgfx_callback_interface_syPKvj'];
var debug_tables = {
  'fi': debug_table_fi,
  'i': debug_table_i,
  'idi': debug_table_idi,
  'ii': debug_table_ii,
  'iid': debug_table_iid,
  'iif': debug_table_iif,
  'iii': debug_table_iii,
  'iiii': debug_table_iiii,
  'iiiii': debug_table_iiiii,
  'iiiiiii': debug_table_iiiiiii,
  'iiiiiiiii': debug_table_iiiiiiiii,
  'iiiiiiiiiiii': debug_table_iiiiiiiiiiii,
  'iiiiiiiiiiiii': debug_table_iiiiiiiiiiiii,
  'iiiiji': debug_table_iiiiji,
  'iiij': debug_table_iiij,
  'iij': debug_table_iij,
  'iijii': debug_table_iijii,
  'ji': debug_table_ji,
  'jiji': debug_table_jiji,
  'v': debug_table_v,
  'vf': debug_table_vf,
  'vff': debug_table_vff,
  'vffff': debug_table_vffff,
  'vfi': debug_table_vfi,
  'vi': debug_table_vi,
  'vif': debug_table_vif,
  'viff': debug_table_viff,
  'vifff': debug_table_vifff,
  'viffff': debug_table_viffff,
  'vii': debug_table_vii,
  'viif': debug_table_viif,
  'viifi': debug_table_viifi,
  'viii': debug_table_viii,
  'viiii': debug_table_viiii,
  'viiiii': debug_table_viiiii,
  'viiiiii': debug_table_viiiiii,
  'viiiiiii': debug_table_viiiiiii,
  'viiiiiiii': debug_table_viiiiiiii,
  'viiiiiiiii': debug_table_viiiiiiiii,
  'viiiiiiiiii': debug_table_viiiiiiiiii,
  'viiiiiiiiiii': debug_table_viiiiiiiiiii,
  'viiiiiiiiiiiiiii': debug_table_viiiiiiiiiiiiiii,
  'viij': debug_table_viij,
  'vijii': debug_table_vijii,
};
function nullFunc_fi(x) { abortFnPtrError(x, 'fi'); }
function nullFunc_i(x) { abortFnPtrError(x, 'i'); }
function nullFunc_idi(x) { abortFnPtrError(x, 'idi'); }
function nullFunc_ii(x) { abortFnPtrError(x, 'ii'); }
function nullFunc_iid(x) { abortFnPtrError(x, 'iid'); }
function nullFunc_iif(x) { abortFnPtrError(x, 'iif'); }
function nullFunc_iii(x) { abortFnPtrError(x, 'iii'); }
function nullFunc_iiii(x) { abortFnPtrError(x, 'iiii'); }
function nullFunc_iiiii(x) { abortFnPtrError(x, 'iiiii'); }
function nullFunc_iiiiiii(x) { abortFnPtrError(x, 'iiiiiii'); }
function nullFunc_iiiiiiiii(x) { abortFnPtrError(x, 'iiiiiiiii'); }
function nullFunc_iiiiiiiiiiii(x) { abortFnPtrError(x, 'iiiiiiiiiiii'); }
function nullFunc_iiiiiiiiiiiii(x) { abortFnPtrError(x, 'iiiiiiiiiiiii'); }
function nullFunc_iiiiji(x) { abortFnPtrError(x, 'iiiiji'); }
function nullFunc_iiij(x) { abortFnPtrError(x, 'iiij'); }
function nullFunc_iij(x) { abortFnPtrError(x, 'iij'); }
function nullFunc_iijii(x) { abortFnPtrError(x, 'iijii'); }
function nullFunc_ji(x) { abortFnPtrError(x, 'ji'); }
function nullFunc_jiji(x) { abortFnPtrError(x, 'jiji'); }
function nullFunc_v(x) { abortFnPtrError(x, 'v'); }
function nullFunc_vf(x) { abortFnPtrError(x, 'vf'); }
function nullFunc_vff(x) { abortFnPtrError(x, 'vff'); }
function nullFunc_vffff(x) { abortFnPtrError(x, 'vffff'); }
function nullFunc_vfi(x) { abortFnPtrError(x, 'vfi'); }
function nullFunc_vi(x) { abortFnPtrError(x, 'vi'); }
function nullFunc_vif(x) { abortFnPtrError(x, 'vif'); }
function nullFunc_viff(x) { abortFnPtrError(x, 'viff'); }
function nullFunc_vifff(x) { abortFnPtrError(x, 'vifff'); }
function nullFunc_viffff(x) { abortFnPtrError(x, 'viffff'); }
function nullFunc_vii(x) { abortFnPtrError(x, 'vii'); }
function nullFunc_viif(x) { abortFnPtrError(x, 'viif'); }
function nullFunc_viifi(x) { abortFnPtrError(x, 'viifi'); }
function nullFunc_viii(x) { abortFnPtrError(x, 'viii'); }
function nullFunc_viiii(x) { abortFnPtrError(x, 'viiii'); }
function nullFunc_viiiii(x) { abortFnPtrError(x, 'viiiii'); }
function nullFunc_viiiiii(x) { abortFnPtrError(x, 'viiiiii'); }
function nullFunc_viiiiiii(x) { abortFnPtrError(x, 'viiiiiii'); }
function nullFunc_viiiiiiii(x) { abortFnPtrError(x, 'viiiiiiii'); }
function nullFunc_viiiiiiiii(x) { abortFnPtrError(x, 'viiiiiiiii'); }
function nullFunc_viiiiiiiiii(x) { abortFnPtrError(x, 'viiiiiiiiii'); }
function nullFunc_viiiiiiiiiii(x) { abortFnPtrError(x, 'viiiiiiiiiii'); }
function nullFunc_viiiiiiiiiiiiiii(x) { abortFnPtrError(x, 'viiiiiiiiiiiiiii'); }
function nullFunc_viij(x) { abortFnPtrError(x, 'viij'); }
function nullFunc_vijii(x) { abortFnPtrError(x, 'vijii'); }

var asmGlobalArg = {}

var asmLibraryArg = {
  "abort": abort,
  "setTempRet0": setTempRet0,
  "getTempRet0": getTempRet0,
  "nullFunc_fi": nullFunc_fi,
  "nullFunc_i": nullFunc_i,
  "nullFunc_idi": nullFunc_idi,
  "nullFunc_ii": nullFunc_ii,
  "nullFunc_iid": nullFunc_iid,
  "nullFunc_iif": nullFunc_iif,
  "nullFunc_iii": nullFunc_iii,
  "nullFunc_iiii": nullFunc_iiii,
  "nullFunc_iiiii": nullFunc_iiiii,
  "nullFunc_iiiiiii": nullFunc_iiiiiii,
  "nullFunc_iiiiiiiii": nullFunc_iiiiiiiii,
  "nullFunc_iiiiiiiiiiii": nullFunc_iiiiiiiiiiii,
  "nullFunc_iiiiiiiiiiiii": nullFunc_iiiiiiiiiiiii,
  "nullFunc_iiiiji": nullFunc_iiiiji,
  "nullFunc_iiij": nullFunc_iiij,
  "nullFunc_iij": nullFunc_iij,
  "nullFunc_iijii": nullFunc_iijii,
  "nullFunc_ji": nullFunc_ji,
  "nullFunc_jiji": nullFunc_jiji,
  "nullFunc_v": nullFunc_v,
  "nullFunc_vf": nullFunc_vf,
  "nullFunc_vff": nullFunc_vff,
  "nullFunc_vffff": nullFunc_vffff,
  "nullFunc_vfi": nullFunc_vfi,
  "nullFunc_vi": nullFunc_vi,
  "nullFunc_vif": nullFunc_vif,
  "nullFunc_viff": nullFunc_viff,
  "nullFunc_vifff": nullFunc_vifff,
  "nullFunc_viffff": nullFunc_viffff,
  "nullFunc_vii": nullFunc_vii,
  "nullFunc_viif": nullFunc_viif,
  "nullFunc_viifi": nullFunc_viifi,
  "nullFunc_viii": nullFunc_viii,
  "nullFunc_viiii": nullFunc_viiii,
  "nullFunc_viiiii": nullFunc_viiiii,
  "nullFunc_viiiiii": nullFunc_viiiiii,
  "nullFunc_viiiiiii": nullFunc_viiiiiii,
  "nullFunc_viiiiiiii": nullFunc_viiiiiiii,
  "nullFunc_viiiiiiiii": nullFunc_viiiiiiiii,
  "nullFunc_viiiiiiiiii": nullFunc_viiiiiiiiii,
  "nullFunc_viiiiiiiiiii": nullFunc_viiiiiiiiiii,
  "nullFunc_viiiiiiiiiiiiiii": nullFunc_viiiiiiiiiiiiiii,
  "nullFunc_viij": nullFunc_viij,
  "nullFunc_vijii": nullFunc_vijii,
  "___cxa_begin_catch": ___cxa_begin_catch,
  "___exception_addRef": ___exception_addRef,
  "___exception_deAdjust": ___exception_deAdjust,
  "___gxx_personality_v0": ___gxx_personality_v0,
  "___lock": ___lock,
  "___setErrNo": ___setErrNo,
  "___syscall140": ___syscall140,
  "___syscall145": ___syscall145,
  "___syscall146": ___syscall146,
  "___syscall221": ___syscall221,
  "___syscall4": ___syscall4,
  "___syscall5": ___syscall5,
  "___syscall54": ___syscall54,
  "___syscall6": ___syscall6,
  "___unlock": ___unlock,
  "__computeUnpackAlignedImageSize": __computeUnpackAlignedImageSize,
  "__emscripten_fetch_xhr": __emscripten_fetch_xhr,
  "__emscripten_get_fetch_work_queue": __emscripten_get_fetch_work_queue,
  "__emscripten_traverse_stack": __emscripten_traverse_stack,
  "__findCanvasEventTarget": __findCanvasEventTarget,
  "__findEventTarget": __findEventTarget,
  "__formatString": __formatString,
  "__glGenObject": __glGenObject,
  "__heapObjectForWebGLType": __heapObjectForWebGLType,
  "__maybeCStringToJsString": __maybeCStringToJsString,
  "__reallyNegative": __reallyNegative,
  "_abort": _abort,
  "_clock": _clock,
  "_clock_gettime": _clock_gettime,
  "_emscripten_asm_const_i": _emscripten_asm_const_i,
  "_emscripten_get_callstack_js": _emscripten_get_callstack_js,
  "_emscripten_get_heap_size": _emscripten_get_heap_size,
  "_emscripten_get_now": _emscripten_get_now,
  "_emscripten_get_now_is_monotonic": _emscripten_get_now_is_monotonic,
  "_emscripten_glActiveTexture": _emscripten_glActiveTexture,
  "_emscripten_glAttachShader": _emscripten_glAttachShader,
  "_emscripten_glBeginQuery": _emscripten_glBeginQuery,
  "_emscripten_glBeginQueryEXT": _emscripten_glBeginQueryEXT,
  "_emscripten_glBeginTransformFeedback": _emscripten_glBeginTransformFeedback,
  "_emscripten_glBindAttribLocation": _emscripten_glBindAttribLocation,
  "_emscripten_glBindBuffer": _emscripten_glBindBuffer,
  "_emscripten_glBindBufferBase": _emscripten_glBindBufferBase,
  "_emscripten_glBindBufferRange": _emscripten_glBindBufferRange,
  "_emscripten_glBindFramebuffer": _emscripten_glBindFramebuffer,
  "_emscripten_glBindRenderbuffer": _emscripten_glBindRenderbuffer,
  "_emscripten_glBindSampler": _emscripten_glBindSampler,
  "_emscripten_glBindTexture": _emscripten_glBindTexture,
  "_emscripten_glBindTransformFeedback": _emscripten_glBindTransformFeedback,
  "_emscripten_glBindVertexArray": _emscripten_glBindVertexArray,
  "_emscripten_glBindVertexArrayOES": _emscripten_glBindVertexArrayOES,
  "_emscripten_glBlendColor": _emscripten_glBlendColor,
  "_emscripten_glBlendEquation": _emscripten_glBlendEquation,
  "_emscripten_glBlendEquationSeparate": _emscripten_glBlendEquationSeparate,
  "_emscripten_glBlendFunc": _emscripten_glBlendFunc,
  "_emscripten_glBlendFuncSeparate": _emscripten_glBlendFuncSeparate,
  "_emscripten_glBlitFramebuffer": _emscripten_glBlitFramebuffer,
  "_emscripten_glBufferData": _emscripten_glBufferData,
  "_emscripten_glBufferSubData": _emscripten_glBufferSubData,
  "_emscripten_glCheckFramebufferStatus": _emscripten_glCheckFramebufferStatus,
  "_emscripten_glClear": _emscripten_glClear,
  "_emscripten_glClearBufferfi": _emscripten_glClearBufferfi,
  "_emscripten_glClearBufferfv": _emscripten_glClearBufferfv,
  "_emscripten_glClearBufferiv": _emscripten_glClearBufferiv,
  "_emscripten_glClearBufferuiv": _emscripten_glClearBufferuiv,
  "_emscripten_glClearColor": _emscripten_glClearColor,
  "_emscripten_glClearDepthf": _emscripten_glClearDepthf,
  "_emscripten_glClearStencil": _emscripten_glClearStencil,
  "_emscripten_glClientWaitSync": _emscripten_glClientWaitSync,
  "_emscripten_glColorMask": _emscripten_glColorMask,
  "_emscripten_glCompileShader": _emscripten_glCompileShader,
  "_emscripten_glCompressedTexImage2D": _emscripten_glCompressedTexImage2D,
  "_emscripten_glCompressedTexImage3D": _emscripten_glCompressedTexImage3D,
  "_emscripten_glCompressedTexSubImage2D": _emscripten_glCompressedTexSubImage2D,
  "_emscripten_glCompressedTexSubImage3D": _emscripten_glCompressedTexSubImage3D,
  "_emscripten_glCopyBufferSubData": _emscripten_glCopyBufferSubData,
  "_emscripten_glCopyTexImage2D": _emscripten_glCopyTexImage2D,
  "_emscripten_glCopyTexSubImage2D": _emscripten_glCopyTexSubImage2D,
  "_emscripten_glCopyTexSubImage3D": _emscripten_glCopyTexSubImage3D,
  "_emscripten_glCreateProgram": _emscripten_glCreateProgram,
  "_emscripten_glCreateShader": _emscripten_glCreateShader,
  "_emscripten_glCullFace": _emscripten_glCullFace,
  "_emscripten_glDeleteBuffers": _emscripten_glDeleteBuffers,
  "_emscripten_glDeleteFramebuffers": _emscripten_glDeleteFramebuffers,
  "_emscripten_glDeleteProgram": _emscripten_glDeleteProgram,
  "_emscripten_glDeleteQueries": _emscripten_glDeleteQueries,
  "_emscripten_glDeleteQueriesEXT": _emscripten_glDeleteQueriesEXT,
  "_emscripten_glDeleteRenderbuffers": _emscripten_glDeleteRenderbuffers,
  "_emscripten_glDeleteSamplers": _emscripten_glDeleteSamplers,
  "_emscripten_glDeleteShader": _emscripten_glDeleteShader,
  "_emscripten_glDeleteSync": _emscripten_glDeleteSync,
  "_emscripten_glDeleteTextures": _emscripten_glDeleteTextures,
  "_emscripten_glDeleteTransformFeedbacks": _emscripten_glDeleteTransformFeedbacks,
  "_emscripten_glDeleteVertexArrays": _emscripten_glDeleteVertexArrays,
  "_emscripten_glDeleteVertexArraysOES": _emscripten_glDeleteVertexArraysOES,
  "_emscripten_glDepthFunc": _emscripten_glDepthFunc,
  "_emscripten_glDepthMask": _emscripten_glDepthMask,
  "_emscripten_glDepthRangef": _emscripten_glDepthRangef,
  "_emscripten_glDetachShader": _emscripten_glDetachShader,
  "_emscripten_glDisable": _emscripten_glDisable,
  "_emscripten_glDisableVertexAttribArray": _emscripten_glDisableVertexAttribArray,
  "_emscripten_glDrawArrays": _emscripten_glDrawArrays,
  "_emscripten_glDrawArraysInstanced": _emscripten_glDrawArraysInstanced,
  "_emscripten_glDrawArraysInstancedANGLE": _emscripten_glDrawArraysInstancedANGLE,
  "_emscripten_glDrawArraysInstancedARB": _emscripten_glDrawArraysInstancedARB,
  "_emscripten_glDrawArraysInstancedEXT": _emscripten_glDrawArraysInstancedEXT,
  "_emscripten_glDrawArraysInstancedNV": _emscripten_glDrawArraysInstancedNV,
  "_emscripten_glDrawBuffers": _emscripten_glDrawBuffers,
  "_emscripten_glDrawBuffersEXT": _emscripten_glDrawBuffersEXT,
  "_emscripten_glDrawBuffersWEBGL": _emscripten_glDrawBuffersWEBGL,
  "_emscripten_glDrawElements": _emscripten_glDrawElements,
  "_emscripten_glDrawElementsInstanced": _emscripten_glDrawElementsInstanced,
  "_emscripten_glDrawElementsInstancedANGLE": _emscripten_glDrawElementsInstancedANGLE,
  "_emscripten_glDrawElementsInstancedARB": _emscripten_glDrawElementsInstancedARB,
  "_emscripten_glDrawElementsInstancedEXT": _emscripten_glDrawElementsInstancedEXT,
  "_emscripten_glDrawElementsInstancedNV": _emscripten_glDrawElementsInstancedNV,
  "_emscripten_glDrawRangeElements": _emscripten_glDrawRangeElements,
  "_emscripten_glEnable": _emscripten_glEnable,
  "_emscripten_glEnableVertexAttribArray": _emscripten_glEnableVertexAttribArray,
  "_emscripten_glEndQuery": _emscripten_glEndQuery,
  "_emscripten_glEndQueryEXT": _emscripten_glEndQueryEXT,
  "_emscripten_glEndTransformFeedback": _emscripten_glEndTransformFeedback,
  "_emscripten_glFenceSync": _emscripten_glFenceSync,
  "_emscripten_glFinish": _emscripten_glFinish,
  "_emscripten_glFlush": _emscripten_glFlush,
  "_emscripten_glFlushMappedBufferRange": _emscripten_glFlushMappedBufferRange,
  "_emscripten_glFramebufferRenderbuffer": _emscripten_glFramebufferRenderbuffer,
  "_emscripten_glFramebufferTexture2D": _emscripten_glFramebufferTexture2D,
  "_emscripten_glFramebufferTextureLayer": _emscripten_glFramebufferTextureLayer,
  "_emscripten_glFrontFace": _emscripten_glFrontFace,
  "_emscripten_glGenBuffers": _emscripten_glGenBuffers,
  "_emscripten_glGenFramebuffers": _emscripten_glGenFramebuffers,
  "_emscripten_glGenQueries": _emscripten_glGenQueries,
  "_emscripten_glGenQueriesEXT": _emscripten_glGenQueriesEXT,
  "_emscripten_glGenRenderbuffers": _emscripten_glGenRenderbuffers,
  "_emscripten_glGenSamplers": _emscripten_glGenSamplers,
  "_emscripten_glGenTextures": _emscripten_glGenTextures,
  "_emscripten_glGenTransformFeedbacks": _emscripten_glGenTransformFeedbacks,
  "_emscripten_glGenVertexArrays": _emscripten_glGenVertexArrays,
  "_emscripten_glGenVertexArraysOES": _emscripten_glGenVertexArraysOES,
  "_emscripten_glGenerateMipmap": _emscripten_glGenerateMipmap,
  "_emscripten_glGetActiveAttrib": _emscripten_glGetActiveAttrib,
  "_emscripten_glGetActiveUniform": _emscripten_glGetActiveUniform,
  "_emscripten_glGetActiveUniformBlockName": _emscripten_glGetActiveUniformBlockName,
  "_emscripten_glGetActiveUniformBlockiv": _emscripten_glGetActiveUniformBlockiv,
  "_emscripten_glGetActiveUniformsiv": _emscripten_glGetActiveUniformsiv,
  "_emscripten_glGetAttachedShaders": _emscripten_glGetAttachedShaders,
  "_emscripten_glGetAttribLocation": _emscripten_glGetAttribLocation,
  "_emscripten_glGetBooleanv": _emscripten_glGetBooleanv,
  "_emscripten_glGetBufferParameteri64v": _emscripten_glGetBufferParameteri64v,
  "_emscripten_glGetBufferParameteriv": _emscripten_glGetBufferParameteriv,
  "_emscripten_glGetBufferPointerv": _emscripten_glGetBufferPointerv,
  "_emscripten_glGetError": _emscripten_glGetError,
  "_emscripten_glGetFloatv": _emscripten_glGetFloatv,
  "_emscripten_glGetFragDataLocation": _emscripten_glGetFragDataLocation,
  "_emscripten_glGetFramebufferAttachmentParameteriv": _emscripten_glGetFramebufferAttachmentParameteriv,
  "_emscripten_glGetInteger64i_v": _emscripten_glGetInteger64i_v,
  "_emscripten_glGetInteger64v": _emscripten_glGetInteger64v,
  "_emscripten_glGetIntegeri_v": _emscripten_glGetIntegeri_v,
  "_emscripten_glGetIntegerv": _emscripten_glGetIntegerv,
  "_emscripten_glGetInternalformativ": _emscripten_glGetInternalformativ,
  "_emscripten_glGetProgramBinary": _emscripten_glGetProgramBinary,
  "_emscripten_glGetProgramInfoLog": _emscripten_glGetProgramInfoLog,
  "_emscripten_glGetProgramiv": _emscripten_glGetProgramiv,
  "_emscripten_glGetQueryObjecti64vEXT": _emscripten_glGetQueryObjecti64vEXT,
  "_emscripten_glGetQueryObjectivEXT": _emscripten_glGetQueryObjectivEXT,
  "_emscripten_glGetQueryObjectui64vEXT": _emscripten_glGetQueryObjectui64vEXT,
  "_emscripten_glGetQueryObjectuiv": _emscripten_glGetQueryObjectuiv,
  "_emscripten_glGetQueryObjectuivEXT": _emscripten_glGetQueryObjectuivEXT,
  "_emscripten_glGetQueryiv": _emscripten_glGetQueryiv,
  "_emscripten_glGetQueryivEXT": _emscripten_glGetQueryivEXT,
  "_emscripten_glGetRenderbufferParameteriv": _emscripten_glGetRenderbufferParameteriv,
  "_emscripten_glGetSamplerParameterfv": _emscripten_glGetSamplerParameterfv,
  "_emscripten_glGetSamplerParameteriv": _emscripten_glGetSamplerParameteriv,
  "_emscripten_glGetShaderInfoLog": _emscripten_glGetShaderInfoLog,
  "_emscripten_glGetShaderPrecisionFormat": _emscripten_glGetShaderPrecisionFormat,
  "_emscripten_glGetShaderSource": _emscripten_glGetShaderSource,
  "_emscripten_glGetShaderiv": _emscripten_glGetShaderiv,
  "_emscripten_glGetString": _emscripten_glGetString,
  "_emscripten_glGetStringi": _emscripten_glGetStringi,
  "_emscripten_glGetSynciv": _emscripten_glGetSynciv,
  "_emscripten_glGetTexParameterfv": _emscripten_glGetTexParameterfv,
  "_emscripten_glGetTexParameteriv": _emscripten_glGetTexParameteriv,
  "_emscripten_glGetTransformFeedbackVarying": _emscripten_glGetTransformFeedbackVarying,
  "_emscripten_glGetUniformBlockIndex": _emscripten_glGetUniformBlockIndex,
  "_emscripten_glGetUniformIndices": _emscripten_glGetUniformIndices,
  "_emscripten_glGetUniformLocation": _emscripten_glGetUniformLocation,
  "_emscripten_glGetUniformfv": _emscripten_glGetUniformfv,
  "_emscripten_glGetUniformiv": _emscripten_glGetUniformiv,
  "_emscripten_glGetUniformuiv": _emscripten_glGetUniformuiv,
  "_emscripten_glGetVertexAttribIiv": _emscripten_glGetVertexAttribIiv,
  "_emscripten_glGetVertexAttribIuiv": _emscripten_glGetVertexAttribIuiv,
  "_emscripten_glGetVertexAttribPointerv": _emscripten_glGetVertexAttribPointerv,
  "_emscripten_glGetVertexAttribfv": _emscripten_glGetVertexAttribfv,
  "_emscripten_glGetVertexAttribiv": _emscripten_glGetVertexAttribiv,
  "_emscripten_glHint": _emscripten_glHint,
  "_emscripten_glInvalidateFramebuffer": _emscripten_glInvalidateFramebuffer,
  "_emscripten_glInvalidateSubFramebuffer": _emscripten_glInvalidateSubFramebuffer,
  "_emscripten_glIsBuffer": _emscripten_glIsBuffer,
  "_emscripten_glIsEnabled": _emscripten_glIsEnabled,
  "_emscripten_glIsFramebuffer": _emscripten_glIsFramebuffer,
  "_emscripten_glIsProgram": _emscripten_glIsProgram,
  "_emscripten_glIsQuery": _emscripten_glIsQuery,
  "_emscripten_glIsQueryEXT": _emscripten_glIsQueryEXT,
  "_emscripten_glIsRenderbuffer": _emscripten_glIsRenderbuffer,
  "_emscripten_glIsSampler": _emscripten_glIsSampler,
  "_emscripten_glIsShader": _emscripten_glIsShader,
  "_emscripten_glIsSync": _emscripten_glIsSync,
  "_emscripten_glIsTexture": _emscripten_glIsTexture,
  "_emscripten_glIsTransformFeedback": _emscripten_glIsTransformFeedback,
  "_emscripten_glIsVertexArray": _emscripten_glIsVertexArray,
  "_emscripten_glIsVertexArrayOES": _emscripten_glIsVertexArrayOES,
  "_emscripten_glLineWidth": _emscripten_glLineWidth,
  "_emscripten_glLinkProgram": _emscripten_glLinkProgram,
  "_emscripten_glMapBufferRange": _emscripten_glMapBufferRange,
  "_emscripten_glPauseTransformFeedback": _emscripten_glPauseTransformFeedback,
  "_emscripten_glPixelStorei": _emscripten_glPixelStorei,
  "_emscripten_glPolygonOffset": _emscripten_glPolygonOffset,
  "_emscripten_glProgramBinary": _emscripten_glProgramBinary,
  "_emscripten_glProgramParameteri": _emscripten_glProgramParameteri,
  "_emscripten_glQueryCounterEXT": _emscripten_glQueryCounterEXT,
  "_emscripten_glReadBuffer": _emscripten_glReadBuffer,
  "_emscripten_glReadPixels": _emscripten_glReadPixels,
  "_emscripten_glReleaseShaderCompiler": _emscripten_glReleaseShaderCompiler,
  "_emscripten_glRenderbufferStorage": _emscripten_glRenderbufferStorage,
  "_emscripten_glRenderbufferStorageMultisample": _emscripten_glRenderbufferStorageMultisample,
  "_emscripten_glResumeTransformFeedback": _emscripten_glResumeTransformFeedback,
  "_emscripten_glSampleCoverage": _emscripten_glSampleCoverage,
  "_emscripten_glSamplerParameterf": _emscripten_glSamplerParameterf,
  "_emscripten_glSamplerParameterfv": _emscripten_glSamplerParameterfv,
  "_emscripten_glSamplerParameteri": _emscripten_glSamplerParameteri,
  "_emscripten_glSamplerParameteriv": _emscripten_glSamplerParameteriv,
  "_emscripten_glScissor": _emscripten_glScissor,
  "_emscripten_glShaderBinary": _emscripten_glShaderBinary,
  "_emscripten_glShaderSource": _emscripten_glShaderSource,
  "_emscripten_glStencilFunc": _emscripten_glStencilFunc,
  "_emscripten_glStencilFuncSeparate": _emscripten_glStencilFuncSeparate,
  "_emscripten_glStencilMask": _emscripten_glStencilMask,
  "_emscripten_glStencilMaskSeparate": _emscripten_glStencilMaskSeparate,
  "_emscripten_glStencilOp": _emscripten_glStencilOp,
  "_emscripten_glStencilOpSeparate": _emscripten_glStencilOpSeparate,
  "_emscripten_glTexImage2D": _emscripten_glTexImage2D,
  "_emscripten_glTexImage3D": _emscripten_glTexImage3D,
  "_emscripten_glTexParameterf": _emscripten_glTexParameterf,
  "_emscripten_glTexParameterfv": _emscripten_glTexParameterfv,
  "_emscripten_glTexParameteri": _emscripten_glTexParameteri,
  "_emscripten_glTexParameteriv": _emscripten_glTexParameteriv,
  "_emscripten_glTexStorage2D": _emscripten_glTexStorage2D,
  "_emscripten_glTexStorage3D": _emscripten_glTexStorage3D,
  "_emscripten_glTexSubImage2D": _emscripten_glTexSubImage2D,
  "_emscripten_glTexSubImage3D": _emscripten_glTexSubImage3D,
  "_emscripten_glTransformFeedbackVaryings": _emscripten_glTransformFeedbackVaryings,
  "_emscripten_glUniform1f": _emscripten_glUniform1f,
  "_emscripten_glUniform1fv": _emscripten_glUniform1fv,
  "_emscripten_glUniform1i": _emscripten_glUniform1i,
  "_emscripten_glUniform1iv": _emscripten_glUniform1iv,
  "_emscripten_glUniform1ui": _emscripten_glUniform1ui,
  "_emscripten_glUniform1uiv": _emscripten_glUniform1uiv,
  "_emscripten_glUniform2f": _emscripten_glUniform2f,
  "_emscripten_glUniform2fv": _emscripten_glUniform2fv,
  "_emscripten_glUniform2i": _emscripten_glUniform2i,
  "_emscripten_glUniform2iv": _emscripten_glUniform2iv,
  "_emscripten_glUniform2ui": _emscripten_glUniform2ui,
  "_emscripten_glUniform2uiv": _emscripten_glUniform2uiv,
  "_emscripten_glUniform3f": _emscripten_glUniform3f,
  "_emscripten_glUniform3fv": _emscripten_glUniform3fv,
  "_emscripten_glUniform3i": _emscripten_glUniform3i,
  "_emscripten_glUniform3iv": _emscripten_glUniform3iv,
  "_emscripten_glUniform3ui": _emscripten_glUniform3ui,
  "_emscripten_glUniform3uiv": _emscripten_glUniform3uiv,
  "_emscripten_glUniform4f": _emscripten_glUniform4f,
  "_emscripten_glUniform4fv": _emscripten_glUniform4fv,
  "_emscripten_glUniform4i": _emscripten_glUniform4i,
  "_emscripten_glUniform4iv": _emscripten_glUniform4iv,
  "_emscripten_glUniform4ui": _emscripten_glUniform4ui,
  "_emscripten_glUniform4uiv": _emscripten_glUniform4uiv,
  "_emscripten_glUniformBlockBinding": _emscripten_glUniformBlockBinding,
  "_emscripten_glUniformMatrix2fv": _emscripten_glUniformMatrix2fv,
  "_emscripten_glUniformMatrix2x3fv": _emscripten_glUniformMatrix2x3fv,
  "_emscripten_glUniformMatrix2x4fv": _emscripten_glUniformMatrix2x4fv,
  "_emscripten_glUniformMatrix3fv": _emscripten_glUniformMatrix3fv,
  "_emscripten_glUniformMatrix3x2fv": _emscripten_glUniformMatrix3x2fv,
  "_emscripten_glUniformMatrix3x4fv": _emscripten_glUniformMatrix3x4fv,
  "_emscripten_glUniformMatrix4fv": _emscripten_glUniformMatrix4fv,
  "_emscripten_glUniformMatrix4x2fv": _emscripten_glUniformMatrix4x2fv,
  "_emscripten_glUniformMatrix4x3fv": _emscripten_glUniformMatrix4x3fv,
  "_emscripten_glUnmapBuffer": _emscripten_glUnmapBuffer,
  "_emscripten_glUseProgram": _emscripten_glUseProgram,
  "_emscripten_glValidateProgram": _emscripten_glValidateProgram,
  "_emscripten_glVertexAttrib1f": _emscripten_glVertexAttrib1f,
  "_emscripten_glVertexAttrib1fv": _emscripten_glVertexAttrib1fv,
  "_emscripten_glVertexAttrib2f": _emscripten_glVertexAttrib2f,
  "_emscripten_glVertexAttrib2fv": _emscripten_glVertexAttrib2fv,
  "_emscripten_glVertexAttrib3f": _emscripten_glVertexAttrib3f,
  "_emscripten_glVertexAttrib3fv": _emscripten_glVertexAttrib3fv,
  "_emscripten_glVertexAttrib4f": _emscripten_glVertexAttrib4f,
  "_emscripten_glVertexAttrib4fv": _emscripten_glVertexAttrib4fv,
  "_emscripten_glVertexAttribDivisor": _emscripten_glVertexAttribDivisor,
  "_emscripten_glVertexAttribDivisorANGLE": _emscripten_glVertexAttribDivisorANGLE,
  "_emscripten_glVertexAttribDivisorARB": _emscripten_glVertexAttribDivisorARB,
  "_emscripten_glVertexAttribDivisorEXT": _emscripten_glVertexAttribDivisorEXT,
  "_emscripten_glVertexAttribDivisorNV": _emscripten_glVertexAttribDivisorNV,
  "_emscripten_glVertexAttribI4i": _emscripten_glVertexAttribI4i,
  "_emscripten_glVertexAttribI4iv": _emscripten_glVertexAttribI4iv,
  "_emscripten_glVertexAttribI4ui": _emscripten_glVertexAttribI4ui,
  "_emscripten_glVertexAttribI4uiv": _emscripten_glVertexAttribI4uiv,
  "_emscripten_glVertexAttribIPointer": _emscripten_glVertexAttribIPointer,
  "_emscripten_glVertexAttribPointer": _emscripten_glVertexAttribPointer,
  "_emscripten_glViewport": _emscripten_glViewport,
  "_emscripten_glWaitSync": _emscripten_glWaitSync,
  "_emscripten_log": _emscripten_log,
  "_emscripten_log_js": _emscripten_log_js,
  "_emscripten_memcpy_big": _emscripten_memcpy_big,
  "_emscripten_performance_now": _emscripten_performance_now,
  "_emscripten_request_animation_frame_loop": _emscripten_request_animation_frame_loop,
  "_emscripten_resize_heap": _emscripten_resize_heap,
  "_emscripten_set_canvas_element_size": _emscripten_set_canvas_element_size,
  "_emscripten_start_fetch": _emscripten_start_fetch,
  "_emscripten_throw_string": _emscripten_throw_string,
  "_emscripten_webgl_create_context": _emscripten_webgl_create_context,
  "_emscripten_webgl_destroy_context": _emscripten_webgl_destroy_context,
  "_emscripten_webgl_destroy_context_calling_thread": _emscripten_webgl_destroy_context_calling_thread,
  "_emscripten_webgl_do_create_context": _emscripten_webgl_do_create_context,
  "_emscripten_webgl_init_context_attributes": _emscripten_webgl_init_context_attributes,
  "_emscripten_webgl_make_context_current": _emscripten_webgl_make_context_current,
  "_exit": _exit,
  "_glActiveTexture": _glActiveTexture,
  "_glAttachShader": _glAttachShader,
  "_glBindBuffer": _glBindBuffer,
  "_glBindFramebuffer": _glBindFramebuffer,
  "_glBindRenderbuffer": _glBindRenderbuffer,
  "_glBindTexture": _glBindTexture,
  "_glBlendColor": _glBlendColor,
  "_glBlendEquationSeparate": _glBlendEquationSeparate,
  "_glBlendFuncSeparate": _glBlendFuncSeparate,
  "_glBufferData": _glBufferData,
  "_glBufferSubData": _glBufferSubData,
  "_glCheckFramebufferStatus": _glCheckFramebufferStatus,
  "_glClear": _glClear,
  "_glClearColor": _glClearColor,
  "_glClearDepthf": _glClearDepthf,
  "_glClearStencil": _glClearStencil,
  "_glColorMask": _glColorMask,
  "_glCompileShader": _glCompileShader,
  "_glCompressedTexImage2D": _glCompressedTexImage2D,
  "_glCompressedTexSubImage2D": _glCompressedTexSubImage2D,
  "_glCreateProgram": _glCreateProgram,
  "_glCreateShader": _glCreateShader,
  "_glCullFace": _glCullFace,
  "_glDeleteBuffers": _glDeleteBuffers,
  "_glDeleteFramebuffers": _glDeleteFramebuffers,
  "_glDeleteProgram": _glDeleteProgram,
  "_glDeleteRenderbuffers": _glDeleteRenderbuffers,
  "_glDeleteShader": _glDeleteShader,
  "_glDeleteTextures": _glDeleteTextures,
  "_glDepthFunc": _glDepthFunc,
  "_glDepthMask": _glDepthMask,
  "_glDetachShader": _glDetachShader,
  "_glDisable": _glDisable,
  "_glDisableVertexAttribArray": _glDisableVertexAttribArray,
  "_glDrawArrays": _glDrawArrays,
  "_glDrawElements": _glDrawElements,
  "_glEnable": _glEnable,
  "_glEnableVertexAttribArray": _glEnableVertexAttribArray,
  "_glFlush": _glFlush,
  "_glFramebufferRenderbuffer": _glFramebufferRenderbuffer,
  "_glFramebufferTexture2D": _glFramebufferTexture2D,
  "_glFrontFace": _glFrontFace,
  "_glGenBuffers": _glGenBuffers,
  "_glGenFramebuffers": _glGenFramebuffers,
  "_glGenRenderbuffers": _glGenRenderbuffers,
  "_glGenTextures": _glGenTextures,
  "_glGenerateMipmap": _glGenerateMipmap,
  "_glGetActiveAttrib": _glGetActiveAttrib,
  "_glGetActiveUniform": _glGetActiveUniform,
  "_glGetAttribLocation": _glGetAttribLocation,
  "_glGetError": _glGetError,
  "_glGetFloatv": _glGetFloatv,
  "_glGetIntegerv": _glGetIntegerv,
  "_glGetProgramInfoLog": _glGetProgramInfoLog,
  "_glGetProgramiv": _glGetProgramiv,
  "_glGetShaderInfoLog": _glGetShaderInfoLog,
  "_glGetShaderiv": _glGetShaderiv,
  "_glGetString": _glGetString,
  "_glGetUniformLocation": _glGetUniformLocation,
  "_glLinkProgram": _glLinkProgram,
  "_glPixelStorei": _glPixelStorei,
  "_glReadPixels": _glReadPixels,
  "_glRenderbufferStorage": _glRenderbufferStorage,
  "_glScissor": _glScissor,
  "_glShaderSource": _glShaderSource,
  "_glStencilFuncSeparate": _glStencilFuncSeparate,
  "_glStencilOpSeparate": _glStencilOpSeparate,
  "_glTexImage2D": _glTexImage2D,
  "_glTexParameterf": _glTexParameterf,
  "_glTexParameterfv": _glTexParameterfv,
  "_glTexParameteri": _glTexParameteri,
  "_glTexSubImage2D": _glTexSubImage2D,
  "_glUniform1i": _glUniform1i,
  "_glUniform1iv": _glUniform1iv,
  "_glUniform4fv": _glUniform4fv,
  "_glUniformMatrix3fv": _glUniformMatrix3fv,
  "_glUniformMatrix4fv": _glUniformMatrix4fv,
  "_glUseProgram": _glUseProgram,
  "_glVertexAttribPointer": _glVertexAttribPointer,
  "_glViewport": _glViewport,
  "_js_html_audioCheckLoad": _js_html_audioCheckLoad,
  "_js_html_audioFree": _js_html_audioFree,
  "_js_html_audioIsPlaying": _js_html_audioIsPlaying,
  "_js_html_audioIsUnlocked": _js_html_audioIsUnlocked,
  "_js_html_audioPause": _js_html_audioPause,
  "_js_html_audioPlay": _js_html_audioPlay,
  "_js_html_audioResume": _js_html_audioResume,
  "_js_html_audioStartLoadFile": _js_html_audioStartLoadFile,
  "_js_html_audioStop": _js_html_audioStop,
  "_js_html_audioUnlock": _js_html_audioUnlock,
  "_js_html_checkLoadImage": _js_html_checkLoadImage,
  "_js_html_finishLoadImage": _js_html_finishLoadImage,
  "_js_html_freeImage": _js_html_freeImage,
  "_js_html_getCanvasSize": _js_html_getCanvasSize,
  "_js_html_getFrameSize": _js_html_getFrameSize,
  "_js_html_getScreenSize": _js_html_getScreenSize,
  "_js_html_imageToMemory": _js_html_imageToMemory,
  "_js_html_init": _js_html_init,
  "_js_html_initAudio": _js_html_initAudio,
  "_js_html_initImageLoading": _js_html_initImageLoading,
  "_js_html_loadImage": _js_html_loadImage,
  "_js_html_setCanvasSize": _js_html_setCanvasSize,
  "_js_inputGetCanvasLost": _js_inputGetCanvasLost,
  "_js_inputGetFocusLost": _js_inputGetFocusLost,
  "_js_inputGetKeyStream": _js_inputGetKeyStream,
  "_js_inputGetMouseStream": _js_inputGetMouseStream,
  "_js_inputGetTouchStream": _js_inputGetTouchStream,
  "_js_inputInit": _js_inputInit,
  "_js_inputResetStreams": _js_inputResetStreams,
  "_llvm_bswap_i64": _llvm_bswap_i64,
  "_llvm_trap": _llvm_trap,
  "abortStackOverflow": abortStackOverflow,
  "demangle": demangle,
  "emscriptenWebGLGet": emscriptenWebGLGet,
  "emscriptenWebGLGetIndexed": emscriptenWebGLGetIndexed,
  "emscriptenWebGLGetTexPixelData": emscriptenWebGLGetTexPixelData,
  "emscriptenWebGLGetUniform": emscriptenWebGLGetUniform,
  "emscriptenWebGLGetVertexAttrib": emscriptenWebGLGetVertexAttrib,
  "flush_NO_FILESYSTEM": flush_NO_FILESYSTEM,
  "jsStackTrace": jsStackTrace,
  "stringToNewUTF8": stringToNewUTF8,
  "warnOnce": warnOnce,
  "tempDoublePtr": tempDoublePtr,
  "DYNAMICTOP_PTR": DYNAMICTOP_PTR
}
// EMSCRIPTEN_START_ASM
var asm =Module["asm"]// EMSCRIPTEN_END_ASM
    
;



// === Auto-generated postamble setup entry stuff ===

if (!Module["intArrayFromString"]) Module["intArrayFromString"] = function() { abort("'intArrayFromString' was not exported. add it to EXTRA_EXPORTED_RUNTIME_METHODS (see the FAQ)") };
if (!Module["intArrayToString"]) Module["intArrayToString"] = function() { abort("'intArrayToString' was not exported. add it to EXTRA_EXPORTED_RUNTIME_METHODS (see the FAQ)") };
if (!Module["ccall"]) Module["ccall"] = function() { abort("'ccall' was not exported. add it to EXTRA_EXPORTED_RUNTIME_METHODS (see the FAQ)") };
if (!Module["cwrap"]) Module["cwrap"] = function() { abort("'cwrap' was not exported. add it to EXTRA_EXPORTED_RUNTIME_METHODS (see the FAQ)") };
if (!Module["setValue"]) Module["setValue"] = function() { abort("'setValue' was not exported. add it to EXTRA_EXPORTED_RUNTIME_METHODS (see the FAQ)") };
if (!Module["getValue"]) Module["getValue"] = function() { abort("'getValue' was not exported. add it to EXTRA_EXPORTED_RUNTIME_METHODS (see the FAQ)") };
if (!Module["allocate"]) Module["allocate"] = function() { abort("'allocate' was not exported. add it to EXTRA_EXPORTED_RUNTIME_METHODS (see the FAQ)") };
if (!Module["getMemory"]) Module["getMemory"] = function() { abort("'getMemory' was not exported. add it to EXTRA_EXPORTED_RUNTIME_METHODS (see the FAQ). Alternatively, forcing filesystem support (-s FORCE_FILESYSTEM=1) can export this for you") };
if (!Module["AsciiToString"]) Module["AsciiToString"] = function() { abort("'AsciiToString' was not exported. add it to EXTRA_EXPORTED_RUNTIME_METHODS (see the FAQ)") };
if (!Module["stringToAscii"]) Module["stringToAscii"] = function() { abort("'stringToAscii' was not exported. add it to EXTRA_EXPORTED_RUNTIME_METHODS (see the FAQ)") };
if (!Module["UTF8ArrayToString"]) Module["UTF8ArrayToString"] = function() { abort("'UTF8ArrayToString' was not exported. add it to EXTRA_EXPORTED_RUNTIME_METHODS (see the FAQ)") };
if (!Module["UTF8ToString"]) Module["UTF8ToString"] = function() { abort("'UTF8ToString' was not exported. add it to EXTRA_EXPORTED_RUNTIME_METHODS (see the FAQ)") };
if (!Module["stringToUTF8Array"]) Module["stringToUTF8Array"] = function() { abort("'stringToUTF8Array' was not exported. add it to EXTRA_EXPORTED_RUNTIME_METHODS (see the FAQ)") };
if (!Module["stringToUTF8"]) Module["stringToUTF8"] = function() { abort("'stringToUTF8' was not exported. add it to EXTRA_EXPORTED_RUNTIME_METHODS (see the FAQ)") };
if (!Module["lengthBytesUTF8"]) Module["lengthBytesUTF8"] = function() { abort("'lengthBytesUTF8' was not exported. add it to EXTRA_EXPORTED_RUNTIME_METHODS (see the FAQ)") };
if (!Module["UTF16ToString"]) Module["UTF16ToString"] = function() { abort("'UTF16ToString' was not exported. add it to EXTRA_EXPORTED_RUNTIME_METHODS (see the FAQ)") };
if (!Module["stringToUTF16"]) Module["stringToUTF16"] = function() { abort("'stringToUTF16' was not exported. add it to EXTRA_EXPORTED_RUNTIME_METHODS (see the FAQ)") };
if (!Module["lengthBytesUTF16"]) Module["lengthBytesUTF16"] = function() { abort("'lengthBytesUTF16' was not exported. add it to EXTRA_EXPORTED_RUNTIME_METHODS (see the FAQ)") };
if (!Module["UTF32ToString"]) Module["UTF32ToString"] = function() { abort("'UTF32ToString' was not exported. add it to EXTRA_EXPORTED_RUNTIME_METHODS (see the FAQ)") };
if (!Module["stringToUTF32"]) Module["stringToUTF32"] = function() { abort("'stringToUTF32' was not exported. add it to EXTRA_EXPORTED_RUNTIME_METHODS (see the FAQ)") };
if (!Module["lengthBytesUTF32"]) Module["lengthBytesUTF32"] = function() { abort("'lengthBytesUTF32' was not exported. add it to EXTRA_EXPORTED_RUNTIME_METHODS (see the FAQ)") };
if (!Module["allocateUTF8"]) Module["allocateUTF8"] = function() { abort("'allocateUTF8' was not exported. add it to EXTRA_EXPORTED_RUNTIME_METHODS (see the FAQ)") };
if (!Module["stackTrace"]) Module["stackTrace"] = function() { abort("'stackTrace' was not exported. add it to EXTRA_EXPORTED_RUNTIME_METHODS (see the FAQ)") };
if (!Module["addOnPreRun"]) Module["addOnPreRun"] = function() { abort("'addOnPreRun' was not exported. add it to EXTRA_EXPORTED_RUNTIME_METHODS (see the FAQ)") };
if (!Module["addOnInit"]) Module["addOnInit"] = function() { abort("'addOnInit' was not exported. add it to EXTRA_EXPORTED_RUNTIME_METHODS (see the FAQ)") };
if (!Module["addOnPreMain"]) Module["addOnPreMain"] = function() { abort("'addOnPreMain' was not exported. add it to EXTRA_EXPORTED_RUNTIME_METHODS (see the FAQ)") };
if (!Module["addOnExit"]) Module["addOnExit"] = function() { abort("'addOnExit' was not exported. add it to EXTRA_EXPORTED_RUNTIME_METHODS (see the FAQ)") };
if (!Module["addOnPostRun"]) Module["addOnPostRun"] = function() { abort("'addOnPostRun' was not exported. add it to EXTRA_EXPORTED_RUNTIME_METHODS (see the FAQ)") };
if (!Module["writeStringToMemory"]) Module["writeStringToMemory"] = function() { abort("'writeStringToMemory' was not exported. add it to EXTRA_EXPORTED_RUNTIME_METHODS (see the FAQ)") };
if (!Module["writeArrayToMemory"]) Module["writeArrayToMemory"] = function() { abort("'writeArrayToMemory' was not exported. add it to EXTRA_EXPORTED_RUNTIME_METHODS (see the FAQ)") };
if (!Module["writeAsciiToMemory"]) Module["writeAsciiToMemory"] = function() { abort("'writeAsciiToMemory' was not exported. add it to EXTRA_EXPORTED_RUNTIME_METHODS (see the FAQ)") };
if (!Module["addRunDependency"]) Module["addRunDependency"] = function() { abort("'addRunDependency' was not exported. add it to EXTRA_EXPORTED_RUNTIME_METHODS (see the FAQ). Alternatively, forcing filesystem support (-s FORCE_FILESYSTEM=1) can export this for you") };
if (!Module["removeRunDependency"]) Module["removeRunDependency"] = function() { abort("'removeRunDependency' was not exported. add it to EXTRA_EXPORTED_RUNTIME_METHODS (see the FAQ). Alternatively, forcing filesystem support (-s FORCE_FILESYSTEM=1) can export this for you") };
if (!Module["ENV"]) Module["ENV"] = function() { abort("'ENV' was not exported. add it to EXTRA_EXPORTED_RUNTIME_METHODS (see the FAQ)") };
if (!Module["FS"]) Module["FS"] = function() { abort("'FS' was not exported. add it to EXTRA_EXPORTED_RUNTIME_METHODS (see the FAQ)") };
if (!Module["FS_createFolder"]) Module["FS_createFolder"] = function() { abort("'FS_createFolder' was not exported. add it to EXTRA_EXPORTED_RUNTIME_METHODS (see the FAQ). Alternatively, forcing filesystem support (-s FORCE_FILESYSTEM=1) can export this for you") };
if (!Module["FS_createPath"]) Module["FS_createPath"] = function() { abort("'FS_createPath' was not exported. add it to EXTRA_EXPORTED_RUNTIME_METHODS (see the FAQ). Alternatively, forcing filesystem support (-s FORCE_FILESYSTEM=1) can export this for you") };
if (!Module["FS_createDataFile"]) Module["FS_createDataFile"] = function() { abort("'FS_createDataFile' was not exported. add it to EXTRA_EXPORTED_RUNTIME_METHODS (see the FAQ). Alternatively, forcing filesystem support (-s FORCE_FILESYSTEM=1) can export this for you") };
if (!Module["FS_createPreloadedFile"]) Module["FS_createPreloadedFile"] = function() { abort("'FS_createPreloadedFile' was not exported. add it to EXTRA_EXPORTED_RUNTIME_METHODS (see the FAQ). Alternatively, forcing filesystem support (-s FORCE_FILESYSTEM=1) can export this for you") };
if (!Module["FS_createLazyFile"]) Module["FS_createLazyFile"] = function() { abort("'FS_createLazyFile' was not exported. add it to EXTRA_EXPORTED_RUNTIME_METHODS (see the FAQ). Alternatively, forcing filesystem support (-s FORCE_FILESYSTEM=1) can export this for you") };
if (!Module["FS_createLink"]) Module["FS_createLink"] = function() { abort("'FS_createLink' was not exported. add it to EXTRA_EXPORTED_RUNTIME_METHODS (see the FAQ). Alternatively, forcing filesystem support (-s FORCE_FILESYSTEM=1) can export this for you") };
if (!Module["FS_createDevice"]) Module["FS_createDevice"] = function() { abort("'FS_createDevice' was not exported. add it to EXTRA_EXPORTED_RUNTIME_METHODS (see the FAQ). Alternatively, forcing filesystem support (-s FORCE_FILESYSTEM=1) can export this for you") };
if (!Module["FS_unlink"]) Module["FS_unlink"] = function() { abort("'FS_unlink' was not exported. add it to EXTRA_EXPORTED_RUNTIME_METHODS (see the FAQ). Alternatively, forcing filesystem support (-s FORCE_FILESYSTEM=1) can export this for you") };
if (!Module["GL"]) Module["GL"] = function() { abort("'GL' was not exported. add it to EXTRA_EXPORTED_RUNTIME_METHODS (see the FAQ)") };
if (!Module["dynamicAlloc"]) Module["dynamicAlloc"] = function() { abort("'dynamicAlloc' was not exported. add it to EXTRA_EXPORTED_RUNTIME_METHODS (see the FAQ)") };
if (!Module["warnOnce"]) Module["warnOnce"] = function() { abort("'warnOnce' was not exported. add it to EXTRA_EXPORTED_RUNTIME_METHODS (see the FAQ)") };
if (!Module["loadDynamicLibrary"]) Module["loadDynamicLibrary"] = function() { abort("'loadDynamicLibrary' was not exported. add it to EXTRA_EXPORTED_RUNTIME_METHODS (see the FAQ)") };
if (!Module["loadWebAssemblyModule"]) Module["loadWebAssemblyModule"] = function() { abort("'loadWebAssemblyModule' was not exported. add it to EXTRA_EXPORTED_RUNTIME_METHODS (see the FAQ)") };
if (!Module["getLEB"]) Module["getLEB"] = function() { abort("'getLEB' was not exported. add it to EXTRA_EXPORTED_RUNTIME_METHODS (see the FAQ)") };
if (!Module["getFunctionTables"]) Module["getFunctionTables"] = function() { abort("'getFunctionTables' was not exported. add it to EXTRA_EXPORTED_RUNTIME_METHODS (see the FAQ)") };
if (!Module["alignFunctionTables"]) Module["alignFunctionTables"] = function() { abort("'alignFunctionTables' was not exported. add it to EXTRA_EXPORTED_RUNTIME_METHODS (see the FAQ)") };
if (!Module["registerFunctions"]) Module["registerFunctions"] = function() { abort("'registerFunctions' was not exported. add it to EXTRA_EXPORTED_RUNTIME_METHODS (see the FAQ)") };
if (!Module["addFunction"]) Module["addFunction"] = function() { abort("'addFunction' was not exported. add it to EXTRA_EXPORTED_RUNTIME_METHODS (see the FAQ)") };
if (!Module["removeFunction"]) Module["removeFunction"] = function() { abort("'removeFunction' was not exported. add it to EXTRA_EXPORTED_RUNTIME_METHODS (see the FAQ)") };
if (!Module["getFuncWrapper"]) Module["getFuncWrapper"] = function() { abort("'getFuncWrapper' was not exported. add it to EXTRA_EXPORTED_RUNTIME_METHODS (see the FAQ)") };
if (!Module["prettyPrint"]) Module["prettyPrint"] = function() { abort("'prettyPrint' was not exported. add it to EXTRA_EXPORTED_RUNTIME_METHODS (see the FAQ)") };
if (!Module["makeBigInt"]) Module["makeBigInt"] = function() { abort("'makeBigInt' was not exported. add it to EXTRA_EXPORTED_RUNTIME_METHODS (see the FAQ)") };
if (!Module["dynCall"]) Module["dynCall"] = function() { abort("'dynCall' was not exported. add it to EXTRA_EXPORTED_RUNTIME_METHODS (see the FAQ)") };
if (!Module["getCompilerSetting"]) Module["getCompilerSetting"] = function() { abort("'getCompilerSetting' was not exported. add it to EXTRA_EXPORTED_RUNTIME_METHODS (see the FAQ)") };
if (!Module["print"]) Module["print"] = function() { abort("'print' was not exported. add it to EXTRA_EXPORTED_RUNTIME_METHODS (see the FAQ)") };
if (!Module["printErr"]) Module["printErr"] = function() { abort("'printErr' was not exported. add it to EXTRA_EXPORTED_RUNTIME_METHODS (see the FAQ)") };
if (!Module["getTempRet0"]) Module["getTempRet0"] = function() { abort("'getTempRet0' was not exported. add it to EXTRA_EXPORTED_RUNTIME_METHODS (see the FAQ)") };
if (!Module["setTempRet0"]) Module["setTempRet0"] = function() { abort("'setTempRet0' was not exported. add it to EXTRA_EXPORTED_RUNTIME_METHODS (see the FAQ)") };if (!Module["ALLOC_NORMAL"]) Object.defineProperty(Module, "ALLOC_NORMAL", { get: function() { abort("'ALLOC_NORMAL' was not exported. add it to EXTRA_EXPORTED_RUNTIME_METHODS (see the FAQ)") } });
if (!Module["ALLOC_STACK"]) Object.defineProperty(Module, "ALLOC_STACK", { get: function() { abort("'ALLOC_STACK' was not exported. add it to EXTRA_EXPORTED_RUNTIME_METHODS (see the FAQ)") } });
if (!Module["ALLOC_DYNAMIC"]) Object.defineProperty(Module, "ALLOC_DYNAMIC", { get: function() { abort("'ALLOC_DYNAMIC' was not exported. add it to EXTRA_EXPORTED_RUNTIME_METHODS (see the FAQ)") } });
if (!Module["ALLOC_NONE"]) Object.defineProperty(Module, "ALLOC_NONE", { get: function() { abort("'ALLOC_NONE' was not exported. add it to EXTRA_EXPORTED_RUNTIME_METHODS (see the FAQ)") } });

function run() {

    var ret = _main();

  checkStackCookie();
}

function initRuntime(asm) {
  runtimeInitialized = true;


  writeStackCookie();

  asm['globalCtors']();

  
}


// Initialize wasm (asynchronous)
var env = asmLibraryArg;
env['memory'] = wasmMemory;
env['table'] = new WebAssembly.Table({ 'initial': 4211
  , 'maximum': 4211
  , 'element': 'anyfunc' });
env['__memory_base'] = STATIC_BASE;
env['__table_base'] = 0;

var imports = {
  'env': env
  , 'global': {
    'NaN': NaN,
    'Infinity': Infinity
  },
  'global.Math': Math,
  'asm2wasm': {
    'f64-rem': function(x, y) { return x % y; },
    'debugger': function() {
      debugger;
    }
  }
};

var ___cxa_demangle,_emscripten_is_main_browser_thread,_free,_htonl,_htons,_llvm_bswap_i16,_llvm_bswap_i32,_main,_malloc,_memalign,_memcpy,_memmove,_memset,_ntohs,_sbrk,_strlen,globalCtors,dynCall_fi,dynCall_i,dynCall_idi,dynCall_ii,dynCall_iid,dynCall_iif,dynCall_iii,dynCall_iiii,dynCall_iiiii,dynCall_iiiiiii,dynCall_iiiiiiiii,dynCall_iiiiiiiiiiii,dynCall_iiiiiiiiiiiii,dynCall_iiiiji,dynCall_iiij,dynCall_iij,dynCall_iijii,dynCall_ji,dynCall_jiji,dynCall_v,dynCall_vf,dynCall_vff,dynCall_vffff,dynCall_vfi,dynCall_vi,dynCall_vif,dynCall_viff,dynCall_vifff,dynCall_viffff,dynCall_vii,dynCall_viif,dynCall_viifi,dynCall_viii,dynCall_viiii,dynCall_viiiii,dynCall_viiiiii,dynCall_viiiiiii,dynCall_viiiiiiii,dynCall_viiiiiiiii,dynCall_viiiiiiiiii,dynCall_viiiiiiiiiii,dynCall_viiiiiiiiiiiiiii,dynCall_viij,dynCall_vijii;

// Streaming Wasm compilation is not possible in Node.js, it does not support the fetch() API.
// In synchronous Wasm compilation mode, Module['wasm'] should contain a typed array of the Wasm object data.
if (!Module['wasm']) throw 'Must load WebAssembly Module in to variable Module.wasm before adding compiled output .js script to the DOM';
Module['wasmInstance'] = WebAssembly.instantiate(Module['wasm'], imports).then(function(output) {
  var asm = output.instance.exports;

  ___cxa_demangle = asm["___cxa_demangle"];
_emscripten_is_main_browser_thread = asm["_emscripten_is_main_browser_thread"];
_free = asm["_free"];
_htonl = asm["_htonl"];
_htons = asm["_htons"];
_llvm_bswap_i16 = asm["_llvm_bswap_i16"];
_llvm_bswap_i32 = asm["_llvm_bswap_i32"];
_main = asm["_main"];
_malloc = asm["_malloc"];
_memalign = asm["_memalign"];
_memcpy = asm["_memcpy"];
_memmove = asm["_memmove"];
_memset = asm["_memset"];
_ntohs = asm["_ntohs"];
_sbrk = asm["_sbrk"];
_strlen = asm["_strlen"];
globalCtors = asm["globalCtors"];
dynCall_fi = asm["dynCall_fi"];
dynCall_i = asm["dynCall_i"];
dynCall_idi = asm["dynCall_idi"];
dynCall_ii = asm["dynCall_ii"];
dynCall_iid = asm["dynCall_iid"];
dynCall_iif = asm["dynCall_iif"];
dynCall_iii = asm["dynCall_iii"];
dynCall_iiii = asm["dynCall_iiii"];
dynCall_iiiii = asm["dynCall_iiiii"];
dynCall_iiiiiii = asm["dynCall_iiiiiii"];
dynCall_iiiiiiiii = asm["dynCall_iiiiiiiii"];
dynCall_iiiiiiiiiiii = asm["dynCall_iiiiiiiiiiii"];
dynCall_iiiiiiiiiiiii = asm["dynCall_iiiiiiiiiiiii"];
dynCall_iiiiji = asm["dynCall_iiiiji"];
dynCall_iiij = asm["dynCall_iiij"];
dynCall_iij = asm["dynCall_iij"];
dynCall_iijii = asm["dynCall_iijii"];
dynCall_ji = asm["dynCall_ji"];
dynCall_jiji = asm["dynCall_jiji"];
dynCall_v = asm["dynCall_v"];
dynCall_vf = asm["dynCall_vf"];
dynCall_vff = asm["dynCall_vff"];
dynCall_vffff = asm["dynCall_vffff"];
dynCall_vfi = asm["dynCall_vfi"];
dynCall_vi = asm["dynCall_vi"];
dynCall_vif = asm["dynCall_vif"];
dynCall_viff = asm["dynCall_viff"];
dynCall_vifff = asm["dynCall_vifff"];
dynCall_viffff = asm["dynCall_viffff"];
dynCall_vii = asm["dynCall_vii"];
dynCall_viif = asm["dynCall_viif"];
dynCall_viifi = asm["dynCall_viifi"];
dynCall_viii = asm["dynCall_viii"];
dynCall_viiii = asm["dynCall_viiii"];
dynCall_viiiii = asm["dynCall_viiiii"];
dynCall_viiiiii = asm["dynCall_viiiiii"];
dynCall_viiiiiii = asm["dynCall_viiiiiii"];
dynCall_viiiiiiii = asm["dynCall_viiiiiiii"];
dynCall_viiiiiiiii = asm["dynCall_viiiiiiiii"];
dynCall_viiiiiiiiii = asm["dynCall_viiiiiiiiii"];
dynCall_viiiiiiiiiii = asm["dynCall_viiiiiiiiiii"];
dynCall_viiiiiiiiiiiiiii = asm["dynCall_viiiiiiiiiiiiiii"];
dynCall_viij = asm["dynCall_viij"];
dynCall_vijii = asm["dynCall_vijii"];


    initRuntime(asm);
    ready();
})
.catch(function(error) {
  console.error(error);
})
;








// {{MODULE_ADDITIONS}}



