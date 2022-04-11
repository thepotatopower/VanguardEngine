-- 剛勇の騎士 レデキウス

function RegisterAbilities()
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.Cont)
	ability1.SetLocation(l.RC)
	ability1.SetActivation("Activation")
end

function Activation()
	if obj.GetNumberOf("Filter", {l.RodeThisTurn}) > 0 then
		obj.AddCardValue({q.Other, o.This}, cs.BonusPower, 5000, p.Continuous)
	end
end

function Filter(snapshot)
	return snapshot.grade >= 3
end