-- 清く正しく ヤコミナ

function RegisterAbilities()
	-- cpnt
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.Cont)
	ability1.SetLocation(l.RC)
	ability1.SetActivation("Activation")
end

function Activation()
	if obj.GetNumberOf("", {l.PlayedOrdersThisTurn}) > 0 then
		obj.AddCardValue({q.Other, o.This}, cs.BonusSkills, s.Boost, p.Continuous)
	end
end
