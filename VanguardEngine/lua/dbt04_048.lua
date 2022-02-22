-- フォーリング・ヘルハザード

function RegisterAbilities()
	-- cont
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.Cont)
	ability1.SetActivation("Cont")
	-- on put
	local ability2 = NewAbility(GetID())
	ability2.SetDescription(2)
	ability2.SetTiming(a.OnPut)
	ability2.SetMovedFrom(l.Order)
	ability2.SetMovedTo(l.Drop)
	ability2.SetTrigger("OnPutTrigger")
	ability2.SetCondition("OnPutCondition")
	ability2.SetActivation("OnPut")
end

function Cont()
	obj.AddCardState({q.Other, o.This}, cs.CountsAsTwoMeteorites, p.Continuous)
end

function OnPutTrigger()
	return obj.IsApplicable()
end

function OnPutCondition()
	return obj.IsSameZone() or obj.CanRetire({q.Location, l.EnemyRC, q.Other, o.CanChoose, q.Count, 1})
end

function OnPut()
	obj.Bind({q.Other, o.ThisFieldID})
	obj.ChooseRetire({q.Location, l.EnemyRC, q.Other, o.CanChoose, q.Count, 1})
end