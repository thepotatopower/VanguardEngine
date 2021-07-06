-- Dragritter, Dabbaax

function NumberOfAbilities()
	return 1
end

function NumberOfParams()
	return 2
end

function GetParam(n)
	if n == 1 then
		return q.Location, l.Damage, q.Count, 2
	elseif n == 2 then
		return q.Location, l.EnemyRC, q.Other, o.CanChoose, q.Count, 1
	end
end

function ActivationRequirement(n)
	if n == 1 then
		return a.PlacedOnRC, t.Auto, p.HasPrompt, true, p.IsMandatory, false
	end
end

function CheckCondition(n)
	if n == 1 then
		if obj.LastPlacedOnRC() and obj.CanCB(1) then
			return true
		end
	end
	return false
end

function Cost(n)
	if n == 1 then
		obj.CounterBlast(1)
	end
end

function Activate(n)
	if n == 1 then
		obj.SoulCharge(1)
		obj.ChooseRetire(2)
	end
	return 0
end