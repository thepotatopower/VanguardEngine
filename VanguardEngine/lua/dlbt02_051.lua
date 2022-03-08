-- ぐるぐるリリック アシュリー

function RegisterAbilities()
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.Cont)
	ability1.SetLocation(l.RC)
	ability1.SetActivation("Activation")
end

function Activation()
	if not obj.Exists({q.Location, l.FrontRowRC, q.Other, o.NotThis}) then
		obj.AddCardState({q.Other, o.This}, cs.Resist, p.Continuous)
		obj.AddCardState({q.Other, o.This}, cs.CannotBeAttacked, p.Continuous)
	end
end