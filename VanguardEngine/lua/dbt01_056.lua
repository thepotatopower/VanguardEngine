-- Stealth Dragon, Hadou Shugen

function NumberOfAbilities()
	return 1
end

function NumberOfParams()
	return 3
end

function GetParam(n)
	if n == 1 then
		return q.Location, l.PlayerRC, q.Location, l.PlayerVC, q.Other, o.OverDress, q.Count, 1
	elseif n == 2 then
		return q.Location, l.Damage, q.Count, 1
	elseif n == 3 then
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
		if obj.LastPlacedOnRC() and obj.Exists(1) and obj.CanCB(2) then
			return true
		end
	end
	return false
end

function CanFullyResolve(n)
	if n == 1 then
		if obj.CanRetire(3) then
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
		obj.ChooseRetire(3)
	end
	return 0
end