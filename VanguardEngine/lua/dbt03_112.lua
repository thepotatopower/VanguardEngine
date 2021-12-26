-- 樹角獣 バロメツ

function RegisterAbilities()
	-- continuous
	local ability1 = NewAbility(GetID())
	ability1.SetTiming(a.Cont)
	ability1.SetLocation(l.VC)
	ability1.SetLocation(l.RC)
	ability1.SetActivation("Activation")
end

function Activation()
	obj.AddCardState({q.Location, l.FrontRow, q.Other, o.This}, cs.CannotAttack, p.Continuous)
end