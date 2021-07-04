-- Sylvan Horned Beast, Giunosla

function NumberOfAbilities()
	return 1
end

function NumberOfParams()
	return 3
end

function GetParam(n)
	if n == 1 then
		return q.Location, l.BackRow, q.Other, o.This
	elseif n == 2 then
		return q.Location, l.Damage, q.Count, 1
	elseif n == 3 then
		return q.Location, l.PlayerRC, q.Other, o.NotThis, q.Count, 1
	end
end

function ActivationRequirement(n)
	if n == 1 then
		return a.OnAttack, t.Auto, p.HasPrompt, true, p.IsMandatory, false
	end
end

function CheckCondition(n)
	if n == 1 then
		if obj.IsAttackingUnit() and obj.CanCB(2) and obj.Exists(1) and obj.Exists(3) then
			return true
		end
	end
	return false
end

function Cost(n)
	if n == 1 then
		obj.CounterBlast(2)
	end
end

function Activate(n)
	if n == 1 then
		obj.ChooseAddTempPower(3, obj.PowerOfThisUnit())
	end
	return 0
end