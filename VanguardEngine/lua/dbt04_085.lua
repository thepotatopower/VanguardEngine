-- 電磁怪獣 エレヒレシーデ

function RegisterAbilities()
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.Cont)
	ability1.SetLocation(l.VC)
	ability1.SetLocation(l.RC)
	ability1.SetActivation("Activation")
end

function Activation()
	if not obj.Exists({q.Location, l.Order}) then
		obj.AddCardState({q.Other, o.This}, cs.CannotAttack, p.Continuous)
	end
end