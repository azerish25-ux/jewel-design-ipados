// Exact source explicit sweep clamp/order fragments; domain/curve stubs are observation aids.
namespace CandidateGeometryContractChecks.Recovered.SweepPreparation;
internal interface IOnCurve{}
internal sealed class DomainCurve{private readonly double first,last;internal DomainCurve(double first,double last){this.first=first;this.last=last;}internal DomainCurve Domain()=>this;internal double Min()=>first;internal double Max()=>last;}
internal static class SourcePlan{
internal static int[] Order(double[] array){var profiles=new IOnCurve[array.Length];
int[] array3 = new int[profiles.Length];
		for (int k = 0; k < profiles.Length; k++)
		{
			array3[k] = k;
		}
		IOnCurve[] array4;
		if (profiles.Length > 1)
		{
			array4 = profiles;
			for (int l = 0; l < array4.Length; l++)
			{
				_ = array4[l];
				for (int m = 0; m < profiles.Length - 1; m++)
				{
					if (array[array3[m]] > array[array3[m + 1]])
					{
						int num = array3[m];
						array3[m] = array3[m + 1];
						array3[m + 1] = num;
					}
				}
			}
		}
return array3;}
internal static (double[],double[]) Clamp(double[] parameters1In,double[] parameters2In,double first1,double last1,double first2,double last2){var profiles=new IOnCurve[parameters1In.Length];var array=new double[profiles.Length];var array2=new double[profiles.Length];var onCurve=new DomainCurve(first1,last1);var onCurve2=new DomainCurve(first2,last2);
for (int j = 0; j < profiles.Length; j++)
			{
				array[j] = parameters1In[j];
				array2[j] = parameters2In[j];
				if (array[j] < onCurve.Domain().Min())
				{
					array[j] = onCurve.Domain().Min();
				}
				if (array[j] > onCurve.Domain().Max())
				{
					array[j] = onCurve.Domain().Max();
				}
				if (array2[j] < onCurve2.Domain().Min())
				{
					array2[j] = onCurve2.Domain().Min();
				}
				if (array2[j] > onCurve2.Domain().Max())
				{
					array2[j] = onCurve2.Domain().Max();
				}
			}
return (array,array2);}
}
