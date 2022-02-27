-- カーディナル・ノイド シュプレマ

function RegisterAbilities()
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.Cont)
	ability1.SetLocation(l.Hand)
	ability1.SetActivation("Activation")
end

function Activation()
	if not obj.IsAbyssalDarkNight() then
		obj.AddCardState({q.Other, o.This}, cs.CannotBeNormalCalled, p.Continuous)
	end
end