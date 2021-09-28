-- Sylvan Horned Beast King, Magnolia

function NumberOfAbilities()
	return 1
end

function NumberOfParams()
	return 3
end

function GetParam(n)
	if n == 1 then
		return q.Location, l.PlayerRC, q.Count, 1
	elseif n == 2 then
		return q.Location, l.PlayerRC, q.Count, 3
	elseif n == 3 then
		return q.Location, l.Selected
	end
end

function ActivationRequirement(n)
	if n == 1 then
		return a.OnBattleEnds, p.HasPrompt, p.CB, 1
	end
end

function CheckCondition(n)
	if n == 1 then
		if obj.IsVanguard() and obj.IsAttackingUnit() then
			return true
		end
	end
	return false
end

function CanFullyResolve(n)
	if n == 1 then
		if obj.Exists(1) then
			return true
		end
	end
	return false
end

function Activate(n)
	if n == 1 then
		if obj.PersonaRode() then
			obj.Select(2)
		else
			obj.Select(1)
		end
		obj.AllowBackRowAttack(3)
		obj.AddTempPower(3, 5000)
		obj.EndSelect()
	end
	return 0
end