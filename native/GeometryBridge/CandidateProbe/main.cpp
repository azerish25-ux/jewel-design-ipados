#include <cstdio>
extern "C" int jd_occt_candidate_run(char*,unsigned int);
int main(){char report[8192]{};int status=jd_occt_candidate_run(report,sizeof(report));std::puts(report);return status;}
