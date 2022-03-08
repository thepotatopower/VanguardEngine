-- 救いの歌を トゥラエル

function RegisterAbilities()
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetProperty(p.WhiteWings)
	ability1.SetTiming(a.Cont)
	ability1.SetLocation(l.GC)
	ability1.SetActivation("Activation")
end

function Activation()
	obj.AddCardValue({q.Other, o.This}, cs.BonusShield, 10000, p.Continuous)
end