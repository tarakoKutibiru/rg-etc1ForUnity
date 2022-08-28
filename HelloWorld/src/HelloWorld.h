#include <iostream>

namespace Hoge
{
    float FooPluginFunction();
    void GetByteArray(unsigned char *ptr);
    void GetIntArray(int *ptr);
}

extern "C"
{
    using debug_log_func_type = void (*)(const char *);

    namespace
    {
        debug_log_func_type debug_log_func = nullptr;
    }

    void helloworld_debug_log(const char *msg)
    {
        if (debug_log_func != nullptr)
            debug_log_func(msg);
    }

    void helloworld_set_debug_log_func(debug_log_func_type func)
    {
        debug_log_func = func;
    }

    void helloworld_debug_log_test()
    {
        helloworld_debug_log("hogehoge");
    }

    float FooPluginFunction()
    {
        return Hoge::FooPluginFunction();
    }

    void helloworld_get_byte(unsigned char *ptr)
    {
        ptr[0] = 122;
    }

    void helloworld_get_byte_array(unsigned char *ptr)
    {
        Hoge::GetByteArray(ptr);
    }

    void helloworld_get_int_array(int *ptr)
    {
        Hoge::GetIntArray(ptr);
    }
}