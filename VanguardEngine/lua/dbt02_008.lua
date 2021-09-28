-- Phantom Blaster Dragon

function NumberOfAbilities()
	return 2
end

function NumberOfParams()
	return 4
end

function GetParam(n)
	if n == 1 then
		return q.Location, l.Soul, q.NameContains, "Blaster", q.Count, 1
	elseif n == 2 then
		return q.Location, l.PlayerRC, q.Count, 3
	elseif n == 3 then
		return q.Location, l.EnemyRC, q.Other, o.CanChoose, q.Count, 2, q.Min, 0
	elseif n == 4 then
		return q.Location, l.PlayerVC, q.Other, o.This
	end
end

function ActivationRequirement(n)
	if n == 1 then
		return a.PlacedOnVC, p.HasPrompt, p.IsMandatory
	elseif n == 2 then
		return a.OnACT, p.HasPrompt, p.OncePerTurn, p.CB, 1, p.Retire, 2
	end
end

function CheckCondition(n)
	if n == 1 then
		if obj.LastPlacedOnVC() and obj.Exists(1) then
			return true
		end
	elseif n == 2 then
		if obj.IsVanguard() then
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

function Activate(n)
	if n == 1 then
		obj.SuperiorCall(1)
	elseif n == 2 then
		obj.ChooseRetire(3)
		obj.AddTempPower(4, 10000)
		obj.AddCritical(4, 1)
	end
	return 0
end
