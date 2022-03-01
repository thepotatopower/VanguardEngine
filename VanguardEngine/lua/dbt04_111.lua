-- 純然の乙女 ベランカ

function RegisterAbilities()
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.Cont)
	ability1.SetLocation(l.GC)
	ability1.SetActivation("Activation")
end

function Activation()
	if obj.Exists({q.Location, l.PlayerVC, q.Grade, 3, q.Other, o.GradeOrHigher}) then
		obj.AddCardValue({q.Other, o.This}, cs.BonusShield, 10000, p.Continuous)
	end
end