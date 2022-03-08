-- 年長の余裕 アルタリエル

function RegisterAbilities()
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.Cont)
	ability1.SetLocation(l.RC)
	ability1.SetActivation("Activation")
end

function Activation()
	if obj.GetNumberOf("Filter", {l.Units}) > 0 then
		obj.AddCardValue({q.Other, o.This}, cs.BonusSkills, s.Boost, p.Continuous)
	end
end

function Filter(id)
	return obj.IsPlayer(id) and obj.IsThis(id) and obj.HasCardState(id, cs.Friend)
end