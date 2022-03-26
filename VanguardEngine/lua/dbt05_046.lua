-- 世話好き怪獣 セコンデル

function RegisterAbilities()
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.PlacedOnRC)
	ability1.SetTrigger("Trigger")
	ability1.SetActivation("Activation")
end

function Trigger()
	return obj.IsApplicable()
end

function Activation()
	obj.Select("Filter", {l.Drop}, 1, 0, {}, -1)
	obj.Store(obj.SendToBottom({q.Location, l.Selected}))
	if obj.Exists({q.Location, l.Stored, q.Grade, 3, q.Other, o.GradeOrHigher}) 
		and obj.CanPayCost("Cost") and obj.PayAdditionalCost() then
		obj.PayCost("Cost")
		obj.Draw(1)
	end
end

function Filter(id)
	return not obj.IsUnitType(id, u.Trigger) 
end

function Cost(check)
	if check then return obj.CanSB(3) end
	obj.SoulBlast(3)
end