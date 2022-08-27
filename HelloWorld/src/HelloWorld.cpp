#include "HelloWorld.h"

namespace Hoge
{
     static int staticIntArray[8] = {0, 0, 0, 0, 1, 1, 1, 1};

     float FooPluginFunction()
     {
          return 5.0f;
     }

     void GetByteArray(unsigned char *ptr)
     {
          for (int i = 0; i < 8; i++)
          {
               ptr[i] = i + 1;
          }
     }

     void GetIntArray(int *ptr)
     {
          for (int i = 0; i < 8; i++)
          {
               ptr[i] = i + 1;
          }
     }
}
