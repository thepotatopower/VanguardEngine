-- Cardinal Noid, Thumborino

function NumberOfAbilities()
	return 2
end

function NumberOfParams()
	return 3
end

function GetParam(n)
	if n == 1 then
		return q.Location, l.PlayerRC, q.Name, "Shadow Army", q.UnitType, u.Token, q.Other, o.Attacking, q.Count, 1
	elseif n == 2 then
		return q.Location, l.PlayerRC, q.Other, o.This
	elseif n == 3 then
		return q.Location, l.PlayerRC, q.Column, 0
	end
end

function ActivationRequirement(n)
	if n == 1 then
		return a.OnAttack, t.Auto, p.HasPrompt, true, p.IsMandatory, false
	elseif n == 2 then
		return a.OnBattleEnds, t.Auto, p.HasPrompt, true, p.IsMandatory, true
	end
end

function CheckCondition(n)
	if n == 1 then
		if obj.IsBooster() and obj.Exists(1) then
			return true
		end
	elseif n == 2 then
		if obj.IsBooster() and obj.Activated(1) then
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
end

function Activate(n)
	if n == 1 then
		obj.AddBattleOnlyPower(2, 15000)
	elseif n == 2 then
		obj.Inject(3, q.Column, obj.GetColumn())
		obj.Retire(3)
		obj.Draw(1)
	end
	return 0
end