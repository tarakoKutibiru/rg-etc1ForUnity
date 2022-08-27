namespace Hoge
{
    float FooPluginFunction();
    void GetByteArray(unsigned int *ptr);
    void GetIntArray(int *ptr);
}

extern "C"
{
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