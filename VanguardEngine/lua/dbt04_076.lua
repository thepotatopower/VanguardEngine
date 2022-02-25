-- スチームパフォーマー ルーガルザ

function RegisterAbilities()
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.Cont)
	ability1.SetLocation(l.GC)
	ability1.SetActivation("Activation")
end

function Activation()
	if obj.Exists({q.Location, l.Soul, q.Grade, 3, q.Count, 2}) then
		obj.AddCardValue({q.Other, o.This}, cs.BonusShield, 5000, p.Continuous)
	end
end