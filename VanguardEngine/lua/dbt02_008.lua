-- Phantom Blaster Dragon

function NumberOfAbilities()
	return 2
end

function NumberOfParams()
	return 5
end

function GetParam(n)
	if n == 1 then
		return q.Location, l.Soul, q.NameContains, "Blaster", q.Count, 1
	elseif n == 2 then
		return q.Location, l.Damage, q.Count, 1
	elseif n == 3 then
		return q.Location, l.PlayerRC, q.Count, 3
	elseif n == 4 then
		return q.Location, l.EnemyRC, q.Other, o.CanChoose, q.Count, 2, q.Min, 0
	elseif n == 5 then
		return q.Location, l.PlayerVC, q.Other, o.This
	end
end

function ActivationRequirement(n)
	if n == 1 then
		return a.PlacedOnVC, t.Auto, p.HasPrompt, true, p.IsMandatory, true
	elseif n == 2 then
		return a.OnACT, t.ACT, p.HasPrompt, true, p.IsMandatory, false, p.CB, 1, p.Retire, 3
	end
end

function CheckCondition(n)
	if n == 1 then
		if obj.LastPlacedOnVC() and obj.Exists(1) then
			return true
		end
	elseif n == 2 then
		if obj.IsVanguard() and not obj.Activated() and obj.CanCB(2) and obj.Exists(3) then
			return true
		end
	end
	return false
end

function CanFullyResolve(n)
	if n == 1 then
		return true
	elseif n == 2 then
		return true
	end
	return false
end

function Cost(n)
	if n == 2 then
		obj.CounterBlast(2)
		obj.ChooseRetire(3)
	end
end

function Activate(n)
	if n == 1 then
		obj.SuperiorCall(1)
	elseif n == 2 then
		obj.ChooseRetire(4)
		obj.AddTempPower(5, 10000)
		obj.AddCritical(5, 1)
	end
	return 0
end
