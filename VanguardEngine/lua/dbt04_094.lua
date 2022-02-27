-- グラビディア・モンデュー

function RegisterAbilities()
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.Cont)
	ability1.SetLocation(l.GC)
	ability1.SetActivation("Activation")
end

function Activation()
	local count = obj.GetNumberOf({q.Location, l.Drop, q.Other, o.Meteorite})
	obj.AddCardValue({q.Other, o.This}, cs.BonusShield, count * 5000, p.Continuous)
end