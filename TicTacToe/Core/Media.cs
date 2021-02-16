using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToe.Core
{
	public class Media
	{
		public static void PlayWinningSound()
		{
			Stream stream = Properties.Resources.win;
			SoundPlayer player = new SoundPlayer(stream);
			player.Play();
		}		
		
		public static void PlayFailingSound()
		{
			Stream stream = Properties.Resources.fail;
			SoundPlayer player = new SoundPlayer(stream);
			player.Play();
		}		
		
		public static void PlayClickingSound()
		{
			Stream stream = Properties.Resources.click;
			SoundPlayer player = new SoundPlayer(stream);
			player.Play();
		}
	}
}
